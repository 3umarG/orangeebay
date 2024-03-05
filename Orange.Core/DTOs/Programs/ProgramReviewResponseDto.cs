namespace Orange_Bay.DTOs.Programs;

public record ProgramReviewResponseDto(
    int UserId,
    string? UserEmail,
    string? UserFullName,
    string? UserPhotoUrl,
    double? Rate,
    string? Description,
    DateTime Date,
    int Id
);