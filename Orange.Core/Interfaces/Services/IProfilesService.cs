using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Profile;

namespace Orange_Bay.Interfaces.Services;

public interface IProfilesService
{
    Task<ProfileResponseDto> GetProfileForUserAsync(int userId);

    Task<ProfileResponseDto> UpdateUserProfileAsync( UpdateProfileDto requestDto);
    Task<AuthModelResponseDto> ChangeUserPasswordAsync(int userId , ChangePasswordRequestDto requestDto);
}