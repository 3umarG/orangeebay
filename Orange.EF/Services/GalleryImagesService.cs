using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Gallery;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Gallery;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class GalleryImagesService : IGalleryImagesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ImageSaver _imageSaver;

    public GalleryImagesService(ApplicationDbContext dbContext, ImageSaver imageSaver)
    {
        _dbContext = dbContext;
        _imageSaver = imageSaver;
    }

    public async Task<List<string>> AddGalleryImageAsync(GalleryImagesRequestDto requestDto)
    {
        if (requestDto.Images.Count == 0) return new List<string>();

        var generatedImages = new List<GalleryImage>();
        for (int i = 0; i < requestDto.Images.Count; i++)
        {
            var image = requestDto.Images[i];
            var imageTypeId = requestDto.ImagesTypeIds[i];

            var imageType = await _dbContext.GalleryImageTypes.FindAsync(imageTypeId);
            if (imageType is null)
            {
                throw new CustomExceptionWithStatusCode(404, $"Not Found Image Type with id : {imageTypeId}");
            }

          
            var photoUrl = await _imageSaver.GenerateImageUrl(image,"gallery");
            var programImage = new GalleryImage
            {
                PhotoUrl = photoUrl,
                TypeId = imageTypeId,
                ImageType = imageType.Type
            };

            generatedImages.Add(programImage);
        }

        await _dbContext.AddRangeAsync(generatedImages);
        await _dbContext.SaveChangesAsync();

        return generatedImages.Select(image => image.PhotoUrl).ToList();
    }

    public async Task<List<GalleryImageResponseDto>> GetAllImagesAsync(int page)
    {
        var galleryImages = page == 0
            ? await GetFullGalleryImages()
            : await GetPaginatedGalleryImages(page);

        return galleryImages;
    }

    private async Task<List<GalleryImageResponseDto>> GetPaginatedGalleryImages(int page)
    {
        return await _dbContext.GalleryImages
            .OrderBy(i => i.Id)
            .Skip((page - 1) * AppUtils.NumberOfItemsPerPage)
            .Take(AppUtils.NumberOfItemsPerPage)
            .Select(image => new GalleryImageResponseDto(image.TypeId, image.ImageType, image.PhotoUrl))
            .ToListAsync();
    }

    private async Task<List<GalleryImageResponseDto>> GetFullGalleryImages()
    {
        return await _dbContext.GalleryImages
            .Select(image => new GalleryImageResponseDto(image.TypeId, image.ImageType, image.PhotoUrl))
            .ToListAsync();
    }

    public async Task<List<string>> AddVideosUrlsAsync(List<string> videosUrls)
    {
        await _dbContext.GalleryVideos.AddRangeAsync(videosUrls.Select(v => new GalleryVideo
        {
            VideoUrl = v
        }));
        await _dbContext.SaveChangesAsync();

        return videosUrls;
    }

    public async Task<List<string>> GetAllVideosUrls(int page)
    {
        var galleryVideos = page == 0
            ? await GetFullGalleryVideos()
            : await GetPaginatedGalleryVideos(page);
        return galleryVideos;
    }

    private async Task<List<string>> GetPaginatedGalleryVideos(int page)
    {
        return await _dbContext.GalleryVideos
            .OrderBy(v => v.Id)
            .Skip((page - 1) * AppUtils.NumberOfItemsPerPage)
            .Take(AppUtils.NumberOfItemsPerPage)
            .Select(v => v.VideoUrl)
            .ToListAsync();
    }

    private async Task<List<string>> GetFullGalleryVideos()
    {
        return await _dbContext.GalleryVideos.Select(v => v.VideoUrl).ToListAsync();
    }
}