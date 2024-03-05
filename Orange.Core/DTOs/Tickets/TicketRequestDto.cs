using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Tickets;

public record TicketRequestDto(
    string FirstName,
    string FamilyName,
    string Phone,
    string Email,
    string Country,
    string City,
    int TicketTypeId,
    List<IFormFile> Images,
    List<string> TicketsIds
);