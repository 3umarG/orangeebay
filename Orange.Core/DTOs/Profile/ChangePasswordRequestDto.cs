namespace Orange_Bay.DTOs.Profile;

public record ChangePasswordRequestDto(
    string OldPassword,
    string NewPassword
    );