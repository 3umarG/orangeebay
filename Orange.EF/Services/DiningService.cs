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

        var diningPhotoUrl = await _imageSaver.GenerateImageUrl(requestDto.Image!, "dining");

        var diningItem = (await _dbContext.DiningItems.AddAsync(
            new DiningItem
            {
                DiningCategoryId = requestDto.DiningCategoryId,
                Description = requestDto.Description,
                Name = requestDto.Name,
                FoodType = requestDto.FoodType,
                PhotoUrl = diningPhotoUrl,
                EndAt = requestDto.EndAt,
                StartFrom = requestDto.StartFrom,
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

    public async Task<DiningItem> UpdateDiningById(int id, DiningRequestDto requestDto)
    {
        var dining = await _dbContext.DiningItems.FindAsync(id);
        if (dining is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found dining with ID : {id}");
        }

        await CheckDiningCategoryExistenceAsync(requestDto.DiningCategoryId);

        dining.Name = requestDto.Name;
        dining.Description = requestDto.Description;
        dining.StartFrom = requestDto.StartFrom;
        dining.EndAt = requestDto.EndAt;
        dining.FoodType = requestDto.FoodType;
        dining.Price = requestDto.Price;
        dining.DiningCategoryId = requestDto.DiningCategoryId;
        if (requestDto.Image is not null)
            dining.PhotoUrl = await _imageSaver.GenerateImageUrl(requestDto.Image, "dining");

        _dbContext.DiningItems.Update(dining);
        await _dbContext.SaveChangesAsync();

        return dining;
    }

    public async Task<DiningItem> DeleteAsync(int id)
    {
        var dining = await _dbContext.DiningItems.FindAsync(id);
        if (dining is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found dining with ID : {id}");
        }

        _dbContext.DiningItems.Remove(dining);
        await _dbContext.SaveChangesAsync();


        return dining;
    }

    public async Task<DiningItem> GetDiningByIdAsync(int id)
    {
        var dining = await _dbContext.DiningItems.FindAsync(id);
        if (dining is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found dining with ID : {id}");
        }

        return dining;
    }
}