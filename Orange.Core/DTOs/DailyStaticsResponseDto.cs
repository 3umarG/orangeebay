namespace Orange_Bay.DTOs;

public record DailyStaticsResponseDto(
    DateTime Date,
    int NumberOfAttendance
);