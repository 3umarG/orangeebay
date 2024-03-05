namespace Orange_Bay.DTOs.Dashboard;

public record DashboardProgramRequestDto(
    string? Name,
    string? Description,
    string? InternalNotes,
    string? SpecialRequirements,
    int MaxCapacity,
    string? Location,
    int DurationInHours,
    int DaysBeforeCancellation,
    List<ProgramPlanRequestDto> ProgramPlans,
    List<ProgramPriceRequestDto> ProgramPrices,
    List<string> ProgramIncludedDetails,
    List<string> ProgramExcludedDetails,
    List<string> ProgramNotes
);

public record ProgramPriceRequestDto(
    double PricePerChild,
    double PricePerAdult,
    DateTime FromDate,
    DateTime ToDate,
    int UserTypeId
);

public record ProgramPlanRequestDto(
    string Time,
    string Description
);