using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.ServicesCount;
using Orange_Bay.Interfaces.Services;

namespace Orange.EF.Services;

public class ServicesCountService : IServicesCountService
{
    private readonly ApplicationDbContext _dbContext;

    public ServicesCountService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServicesCountResponseDto> GetAllServicesItemsCount()
    {
        var restaurantNumber = await _dbContext.DiningItems.CountAsync();
        var imagesNumber = await _dbContext.GalleryImages.CountAsync();
        var videosNumber = await _dbContext.GalleryVideos.CountAsync();
        var activitiesNumber = 0; // TODO : will added it later
        
        return new ServicesCountResponseDto(imagesNumber,videosNumber,restaurantNumber,activitiesNumber);
    }
}