using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.DTOs.Tickets;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Tickets;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class TicketsService : ITicketsService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ImageSaver _imageSaver;
    public TicketsService(ApplicationDbContext dbContext, ImageSaver imageSaver)
    {
        _dbContext = dbContext;
        _imageSaver = imageSaver;
    }

    public async Task<TicketDetails> AddTicketAsync(TicketRequestDto dto)
    {
        if (dto.Images.Count != dto.TicketsIds.Count)
        {
            throw new CustomExceptionWithStatusCode(400, "Please add all TicketsIds !!");
        }

        var ticketType = await _dbContext.TicketsTypes.FindAsync(dto.TicketTypeId);
        if (ticketType is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Ticket Type with ID :{dto.TicketTypeId}");
        }

        switch (dto)
        {
            case { TicketTypeId: 2, Images.Count: < 2 }:
                throw new CustomExceptionWithStatusCode(400, "Double Ticket Type should include 2 Tickets !!");
            case { TicketTypeId: 3, Images.Count: < 2 }:
                throw new CustomExceptionWithStatusCode(400, "Family Ticket Type should include minimum 2 Tickets !!");
            case { TicketTypeId: 4, Images.Count: < 6 }:
                throw new CustomExceptionWithStatusCode(400, "Group Ticket Type should include minimum 6 Tickets !!");
        }

        var generatedTicketImages = new List<TicketImage>();
        for (var i = 0; i < dto.Images.Count; i++)
        {
            var ticketImage = dto.Images[i];
            var ticketId = dto.TicketsIds[i];

            var photoUrl = await _imageSaver.GenerateImageUrl(ticketImage, "tickets");
            var programImage = new TicketImage
            {
                PhotoUrl = photoUrl,
                TicketId = ticketId
            };

            generatedTicketImages.Add(programImage);
        }


        var ticket = new TicketDetails
        {
            FirstName = dto.FirstName,
            FamilyName = dto.FamilyName,
            CreatedOn = DateTime.Today,
            City = dto.City,
            Country = dto.Country,
            Email = dto.Email,
            Phone = dto.Phone,
            TicketTypeId = dto.TicketTypeId,
            TicketImages = generatedTicketImages
        };

        await _dbContext.TicketsDetails.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();

        return ticket;
    }

    public async Task<PaginatedResponseDto<TicketDetails>> GetAllTicketsDetailsAsync(int page, DateTime? date = null)
    {
        var tickets = await _dbContext.TicketsDetails
            .Include(t => t.TicketImages)
            .Include(t => t.TicketType)
            .Where(t => date == null || t.CreatedOn.Date == date.Value.Date)
            .OrderBy(t => t.CreatedOn)
            .Skip((page - 1) * AppUtils.NumberOfTicketsPerPage)
            .Take(AppUtils.NumberOfTicketsPerPage)
            .ToListAsync();
        
        var ticketsCount =
            _dbContext.TicketsDetails.Count(t => date == null || t.CreatedOn.Date == date.Value.Date);
        var pages = (int)Math.Ceiling((double)ticketsCount / AppUtils.NumberOfTicketsPerPage);

        return new PaginatedResponseDto<TicketDetails>
        {
            Items = tickets,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<List<TicketType>> GetAllTicketsTypesAsync()
    {
        return await _dbContext.TicketsTypes.ToListAsync();
    }
}