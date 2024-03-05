namespace Orange_Bay.DTOs.AdditionalServices;

public record AdditionalServiceResponseDto(
    int Id,
    string Name,
    string Description,
    double PricePerChild,
    double PricePerAdult
    );