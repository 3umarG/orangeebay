using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.SliderImage;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SliderImagesController
{
    private readonly ISliderImagesService _sliderImagesService;

    public SliderImagesController(ISliderImagesService sliderImagesService)
    {
        _sliderImagesService = sliderImagesService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllSliderImagesAsync()
    {
        var result = await _sliderImagesService.GetAllSliderImagesAsync();
        var responseBody = ResponseModelDto<List<SliderImage>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}