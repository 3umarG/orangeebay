using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Tickets;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Tickets;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TicketsController
{
    private readonly ITicketsService _ticketsService;

    public TicketsController(ITicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTicketAsync([FromForm] TicketRequestDto requestDto)
    {
        var result = await _ticketsService.AddTicketAsync(requestDto);
        var responseBody = ResponseModelDto<TicketDetails>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Types")]
    public async Task<IActionResult> GetAllTicketsTypesAsync()
    {
        var result = await _ticketsService.GetAllTicketsTypesAsync();
        var responseBody = ResponseModelDto<List<TicketType>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}