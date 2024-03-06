using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Dining;

public record DiningRequestDto(
    string Name,
    string Description,
    string StartFrom,
    string EndAt,
    int DiningCategoryId,
    string FoodType,
    IFormFile? Image,
    double Price
    );