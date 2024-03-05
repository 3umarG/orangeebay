using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.SliderImage;

namespace Orange.EF.Services;

public class SliderImagesService : ISliderImagesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ImageSaver _imageSaver;

    public SliderImagesService(ApplicationDbContext dbContext, ImageSaver imageSaver)
    {
        _dbContext = dbContext;
        _imageSaver = imageSaver;
    }

    public async Task<List<SliderImage>> AddSliderImagesAsync(ImagesRequestDto imagesRequestDto)
    {
        if (imagesRequestDto.Images.Count == 0) return new List<SliderImage>();

        var generatedImages = new List<SliderImage>();
        foreach (var imageFile in imagesRequestDto.Images)
        {

            var photoUrl = await _imageSaver.GenerateImageUrl(imageFile, "sliders");
            var programImage = new SliderImage
            {
                PhotoUrl = photoUrl
            };

            generatedImages.Add(programImage);
        }

        await _dbContext.SliderImages.AddRangeAsync(generatedImages);
        await _dbContext.SaveChangesAsync();

        return generatedImages;
    }

    public async Task<List<SliderImage>> GetAllSliderImagesAsync()
    {
        return await _dbContext.SliderImages.ToListAsync();
    }

    public async Task<SliderImage> DeleteSliderImageAsync(int id)
    {
        var image = await _dbContext.SliderImages.FindAsync(id);
        if (image is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Slider Image with ID : {id}");
        }

        _dbContext.SliderImages.Remove(image);
        await _dbContext.SaveChangesAsync();

        return image;
    }
}