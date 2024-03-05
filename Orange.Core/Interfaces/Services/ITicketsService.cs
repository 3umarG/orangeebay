using Orange_Bay.DTOs.Shared;
using Orange_Bay.DTOs.Tickets;
using Orange_Bay.Models.Tickets;

namespace Orange_Bay.Interfaces.Services;

public interface ITicketsService
{
    Task<TicketDetails> AddTicketAsync(TicketRequestDto dto);
    Task<PaginatedResponseDto<TicketDetails>> GetAllTicketsDetailsAsync(int page, DateTime? date = null);
    Task<List<TicketType>> GetAllTicketsTypesAsync();
}