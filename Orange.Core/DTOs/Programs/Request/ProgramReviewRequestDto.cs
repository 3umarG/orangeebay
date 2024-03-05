namespace Orange_Bay.DTOs.Programs.Request;


public record ProgramReviewRequestDto(
    int ProgramId,
    int UserId,
    string Review,
    double RateFromFive
);