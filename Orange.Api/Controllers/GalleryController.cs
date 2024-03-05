using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Gallery;
using Orange_Bay.Interfaces.Services;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GalleryController
{
    private readonly IGalleryImagesService _galleryImagesService;

    public GalleryController(IGalleryImagesService galleryImagesService)
    {
        _galleryImagesService = galleryImagesService;
    }

    [HttpPost]
    public async Task<IActionResult> AddGalleryImageAsync([FromForm] GalleryImagesRequestDto requestDto)
    {
        var result = await _galleryImagesService.AddGalleryImageAsync(requestDto);
        var response = ResponseModelDto<List<string>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGalleryImagesAsync([FromQuery] int page = 0)
    {
        var result = await _galleryImagesService.GetAllImagesAsync(page);
        var response = ResponseModelDto<List<GalleryImageResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPost("Videos")]
    public async Task<IActionResult> AddVideosAsync(VideosUrlsRequestDto requestDto)
    {
        var result = await _galleryImagesService.AddVideosUrlsAsync(requestDto.Videos);
        var responseBody = ResponseModelDto<List<string>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }

    [HttpGet("Videos")]
    public async Task<IActionResult> GetAllVideosUrlsAsync([FromQuery] int page = 0)
    {
        var result = await _galleryImagesService.GetAllVideosUrls(page);
        var responseBody = ResponseModelDto<List<string>>.BuildSuccessResponse(result);
        return new OkObjectResult(responseBody);
    }
}