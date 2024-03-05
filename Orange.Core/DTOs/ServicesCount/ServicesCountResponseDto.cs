namespace Orange_Bay.DTOs.ServicesCount;

public record ServicesCountResponseDto(
    int NumberOfImages,
    int NumberOfVideos,
    int NumberOfRestaurant,
    int NumberOfActivities
);