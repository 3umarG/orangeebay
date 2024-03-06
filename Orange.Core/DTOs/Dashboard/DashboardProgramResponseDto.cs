namespace Orange_Bay.DTOs.Dashboard;

public record DashboardProgramResponseDto(
    int Id,
    string? Name,
    string? Description,
    string? InternalNotes,
    string? SpecialRequirements,
    int MaxCapacity,
    string? Location,
    int DurationInHours,
    int? DaysBeforeCancellation,
    List<ProgramPlanResponseDto> ProgramPlans,
    List<ProgramPriceResponseDto> ProgramPrices,
    List<string> ProgramIncludedDetails,
    List<string> ProgramExcludedDetails,
    List<string> ProgramNotes
);

public record ProgramPlanResponseDto(
    int Id,
    string Time,
    string Description
);

public record ProgramPriceResponseDto(
    int Id,
    double PricePerChild,
    double PricePerAdult,
    DateTime FromDate,
    DateTime ToDate,
    int UserTypeId
);