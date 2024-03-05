using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Dining;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Dining;

namespace Orange.EF.Services;

public class DiningService : IDiningService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ImageSaver _imageSaver;

    public DiningService(ApplicationDbContext dbContext, ImageSaver imageSaver)
    {
        _dbContext = dbContext;
        _imageSaver = imageSaver;
    }

    public async Task<DiningItem> AddDiningItemAsync(DiningRequestDto requestDto)
    {
        await CheckDiningCategoryExistenceAsync(requestDto.DiningCategoryId);

        var diningPhotoUrl = await _imageSaver.GenerateImageUrl(requestDto.Image, "dining");

        var diningItem = (await _dbContext.DiningItems.AddAsync(
            new DiningItem
            {
                DiningCategoryId = requestDto.DiningCategoryId,
                Description = requestDto.Description,
                Name = requestDto.Name,
                FoodType = requestDto.FoodType,
                PhotoUrl = diningPhotoUrl,
                EndAt = requestDto.EndTime,
                StartFrom = requestDto.StartTime,
                Price = requestDto.Price
            })).Entity;

        await _dbContext.SaveChangesAsync();

        return diningItem;
    }

    private async Task CheckDiningCategoryExistenceAsync(int id)
    {
        if (!await _dbContext.DiningCategories.AnyAsync(category => category.Id == id))
        {
            throw new CustomExceptionWithStatusCode(404,
                $"Not Found Dining Category with id : {id}");
        }
    }

    public async Task<List<DiningItem>> GetAllDiningItemsAsync(int? categoryId)
    {
        if (categoryId == null) return await _dbContext.DiningItems.ToListAsync();

        await CheckDiningCategoryExistenceAsync((int)categoryId);
        var diningItemsByCategoryId =
            await _dbContext.DiningItems
                .Where(item => item.DiningCategoryId == categoryId)
                .ToListAsync();
        return diningItemsByCategoryId;
    }
}