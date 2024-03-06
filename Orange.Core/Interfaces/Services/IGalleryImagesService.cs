using Orange_Bay.DTOs.Gallery;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.DTOs.Shared;

namespace Orange_Bay.Interfaces.Services;

public interface IGalleryImagesService
{
    Task<List<string>> AddGalleryImageAsync(GalleryImagesRequestDto requestDto);

    Task<PaginatedResponseDto<GalleryImageResponseDto>> GetAllImagesAsync(int page);
    Task<List<string>> AddVideosUrlsAsync(List<string> videosUrls);
    Task<List<string>> GetAllVideosUrls(int page);
}