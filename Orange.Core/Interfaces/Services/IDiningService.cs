using Orange_Bay.DTOs.Dining;
using Orange_Bay.Models.Dining;

namespace Orange_Bay.Interfaces.Services;

public interface IDiningService
{
    Task<DiningItem> AddDiningItemAsync(DiningRequestDto requestDto);
    Task<List<DiningItem>> GetAllDiningItemsAsync(int? categoryId);
    Task<DiningItem> UpdateDiningById(int id, DiningRequestDto requestDto);
    Task<DiningItem> DeleteAsync(int id);
    Task<DiningItem> GetDiningByIdAsync(int id);
}