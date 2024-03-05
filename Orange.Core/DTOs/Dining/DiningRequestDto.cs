using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Dining;

public record DiningRequestDto(
    string Name,
    string Description,
    string StartTime,
    string EndTime,
    int DiningCategoryId,
    string FoodType,
    IFormFile Image,
    double Price
    );