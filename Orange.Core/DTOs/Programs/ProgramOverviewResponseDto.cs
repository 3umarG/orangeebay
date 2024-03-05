namespace Orange_Bay.DTOs.Programs;

public record ProgramOverviewResponseDto(
    int Id,
    string Name,
    int DurationInHours,
    string? Location,
    string? Description,
    string? InternalNotes,
    string? SpecialRequirements,
    double PricePerChild,
    double PricePerAdult,
    DateTime FromDate,
    DateTime ToDate,
    List<string> Photos,
    List<string> Notes,
    int MaxCapacity
);