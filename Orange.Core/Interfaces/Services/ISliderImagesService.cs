using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.Models.SliderImage;

namespace Orange_Bay.Interfaces.Services;

public interface ISliderImagesService
{
    Task<List<SliderImage>> AddSliderImagesAsync(ImagesRequestDto imagesRequestDto);
    Task<List<SliderImage>> GetAllSliderImagesAsync();
    Task<SliderImage> DeleteSliderImageAsync(int id);
}