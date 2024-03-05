using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.ServicesCount;
using Orange_Bay.Interfaces.Services;
using Orange.EF;
using Orange.EF.Services;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController
{
    private readonly IServicesCountService _servicesCountService;

    public LookupsController(ApplicationDbContext context)
    {
        _servicesCountService = new ServicesCountService(context);
    }

    [HttpGet("Services/Count")]
    public async Task<IActionResult> GetAllServicesCountAsync()
    {
        var result = await _servicesCountService.GetAllServicesItemsCount();
        var response = ResponseModelDto<ServicesCountResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }
}