using Orange_Bay.DTOs.Gallery;
using Orange_Bay.DTOs.Programs.Request;

namespace Orange_Bay.Interfaces.Services;

public interface IGalleryImagesService
{
    Task<List<string>> AddGalleryImageAsync(GalleryImagesRequestDto requestDto);

    Task<List<GalleryImageResponseDto>> GetAllImagesAsync(int page);
    Task<List<string>> AddVideosUrlsAsync(List<string> videosUrls);
    Task<List<string>> GetAllVideosUrls(int page);
}