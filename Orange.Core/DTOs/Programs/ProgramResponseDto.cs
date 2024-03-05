namespace Orange_Bay.DTOs.Programs;

public record ProgramResponseDto(
    int Id,
    List<string> Photos,
    string? Name,
    double? Rate,
    double? PricePerChild,
    double? PricePerAdult,
    bool IsFavourite,
    DateTime? FromDate,
    DateTime? ToDate
);