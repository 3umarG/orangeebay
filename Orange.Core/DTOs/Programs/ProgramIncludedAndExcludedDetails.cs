namespace Orange_Bay.DTOs.Programs;

public record ProgramIncludedAndExcludedDetails(
    List<string> Included,
    List<string> Excluded
    );