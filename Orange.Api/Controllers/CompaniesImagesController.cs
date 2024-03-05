using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.CompanyImages;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CompaniesImagesController
{
    private readonly ICompaniesImagesService _imagesService;

    public CompaniesImagesController(ICompaniesImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompaniesImagesAsync()
    {
        var result = await _imagesService.GetAllCompanyImagesAsync();
        var responseBody = ResponseModelDto<List<CompanyImage>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}