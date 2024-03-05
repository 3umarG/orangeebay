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
    List<ProgramPlanRequestDto> ProgramPlans,
    List<ProgramPriceRequestDto> ProgramPrices,
    List<string> ProgramIncludedDetails,
    List<string> ProgramExcludedDetails,
    List<string> ProgramNotes
    );