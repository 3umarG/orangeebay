using Microsoft.AspNetCore.Mvc;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Repositories;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.CompanyImages;
using Orange_Bay.Models.Gallery;
using Orange_Bay.Models.SliderImage;
using Orange_Bay.Models.Tickets;
using Orange.EF;
using Orange.EF.Repositories.Impl;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IBaseImagesRepository<GalleryImage> _galleryImagesRepository;
    private readonly IBaseImagesRepository<SliderImage> _sliderImagesRepository;
    private readonly IBaseImagesRepository<TicketImage> _ticketsImagesRepository;
    private readonly IBaseImagesRepository<CompanyImage> _companiesImagesRepository;

    public ImagesController(ApplicationDbContext context)
    {
        new ProgramsImagesRepository(context);
        _galleryImagesRepository = new GalleryImagesRepository(context);
        _sliderImagesRepository = new SliderImagesRepository(context);
        _ticketsImagesRepository = new TicketsImagesRepository(context);
        _companiesImagesRepository = new CompaniesImagesRepository(context);
    }

    // [HttpGet("Programs/{title}")]
    // public async Task<IActionResult> GetProgramImageAsync(string title)
    // {
    //     var programImage = await _programsImagesRepository.FindByAsync(image => image.Title == title);
    //     if (programImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(programImage.Data, "image/jpeg");
    // }
    
    // [HttpGet("Gallery/{title}")]
    // public async Task<IActionResult> GetGalleryImageAsync(string title)
    // {
    //     var programImage = await _galleryImagesRepository.FindByAsync(image => image.Title == title);
    //     if (programImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(programImage.Data, "image/jpeg");
    // }
    
    // [HttpGet("Dining/{title}")]
    // public async Task<IActionResult> GetDiningImageAsync(string title)
    // {
    //     var programImage = await _diningImagesRepository.FindByAsync(image => image.Title == title);
    //     if (programImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(programImage.Data, "image/jpeg");
    // }

    // [HttpGet("Slider/{title}")]
    // public async Task<IActionResult> GetSliderImageAsync(string title)
    // {
    //     var sliderImage = await _sliderImagesRepository.FindByAsync(image => image.Title == title);
    //     if (sliderImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(sliderImage.Data, "image/jpeg");
    // }
    
    // [HttpGet("Tickets/{title}")]
    // public async Task<IActionResult> GetTicketsImageAsync(string title)
    // {
    //     var sliderImage = await _ticketsImagesRepository.FindByAsync(image => image.Title == title);
    //     if (sliderImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(sliderImage.Data, "image/jpeg");
    // }
    //
    // [HttpGet("Company/{title}")]
    // public async Task<IActionResult> GetCompanyImageAsync(string title)
    // {
    //     var sliderImage = await _companiesImagesRepository.FindByAsync(image => image.Title == title);
    //     if (sliderImage is null)
    //     {
    //         throw new CustomExceptionWithStatusCode(404, "Not Found Image with Title !!");
    //     }
    //
    //     return File(sliderImage.Data, "image/jpeg");
    // }

}