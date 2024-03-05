using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.AdditionalServices;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.Interfaces.Services;
using Orange.EF;
using Orange.EF.Services;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AdditionalServicesController
{
    private readonly IAdditionalServicesService _additionalServices;

    public AdditionalServicesController(ApplicationDbContext context)
    {
        _additionalServices = new AdditionalServicesService(context);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAdditionalServicesByUserTypeIdAsync(int userTypeId)
    {
        var result = await _additionalServices.GetAllAdditionalServicesByUserTypeIdAsync(userTypeId);
        var responseBody = ResponseModelDto<List<AdditionalServiceResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}