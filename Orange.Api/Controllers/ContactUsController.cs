using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.ContactUs;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.ContactUs;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactUsController
{
    private readonly IContactUsService _contactUsService;

    public ContactUsController(IContactUsService contactUsService)
    {
        _contactUsService = contactUsService;
    }

    [HttpPost]
    public async Task<IActionResult> AddContactUsMessageAsync([FromBody] ContactUsMessageRequestDto dto)
    {
        var result = await _contactUsService.AddContactUsMessageAsync(dto);
        var responseBody = ResponseModelDto<ContactUsMessage>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }


    [HttpGet]
    public async Task<IActionResult> GetContactUsMessagesAsync([FromQuery] DateTime? date)
    {
        var result = await _contactUsService.GetContactUsMessagesAsync(date);
        var responseBody = ResponseModelDto<IEnumerable<ContactUsMessage>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}