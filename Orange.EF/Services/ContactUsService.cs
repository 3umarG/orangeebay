using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.ContactUs;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.ContactUs;

namespace Orange.EF.Services;

public class ContactUsService : IContactUsService
{
    private readonly ApplicationDbContext _dbContext;

    public ContactUsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContactUsMessage> AddContactUsMessageAsync(ContactUsMessageRequestDto dto)
    {
        var contactUsMessage = (await _dbContext.ContactUsMessages.AddAsync(
            new ContactUsMessage
            {
                Email = dto.Email,
                Message = dto.Message,
                Phone = dto.Phone,
                Subject = dto.Subject,
                FullName = dto.FullName
            })).Entity;
        await _dbContext.SaveChangesAsync();

        return contactUsMessage;
    }

    public async Task<IEnumerable<ContactUsMessage>> GetContactUsMessagesAsync(DateTime? date)
    {
        var messages = await _dbContext.ContactUsMessages
            .Where(message =>
                !date.HasValue || message.CreatedOn.Date == date.Value.Date)
            .ToListAsync();

        return messages;
    }
}