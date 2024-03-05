namespace Orange_Bay.DTOs.Dashboard;

public record DashboardAuthModelResponseDto(
    int Id,
    string Email,
    string? Token,
    DateTime AccessTokenExpiration,
    string? Role
);