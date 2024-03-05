using Microsoft.AspNetCore.Identity;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Profile;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Auth;

namespace Orange.EF.Services;

public class ProfilesService : IProfilesService
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfilesService(IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }


    public async Task<ProfileResponseDto> GetProfileForUserAsync(int userId)
    {
        var user = await FindUserByIdAsync(userId);

        return new ProfileResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhotoUrl = user.PhotoUrl,
            Phone = user.PhoneNumber,
            UserName = user.UserName,
            UserTypeId = user.UserTypeId
        };
    }

    private async Task<ApplicationUser> FindUserByIdAsync(int userId)
    {
        var user = await _authService.FindUserByIdAsync(userId);

        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {userId}");
        }

        return user;
    }

    public async Task<ProfileResponseDto> UpdateUserProfileAsync(UpdateProfileDto requestDto)
    {
        var user = await FindUserByIdAsync(requestDto.UserId);

        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {requestDto.UserId}");
        }

        var userWithSpecifiedEmail = await _userManager.FindByEmailAsync(requestDto.Email);
        if (userWithSpecifiedEmail is not null&&user.Email!=requestDto.Email)
        {
            throw new CustomExceptionWithStatusCode(400, $"There is already user with email : {requestDto.Email}");
        }

        user.Email = requestDto.Email;
        user.FullName = requestDto.FullName;
        user.PhoneNumber = requestDto.Phone??"";
        await _userManager.UpdateAsync(user);
        return new ProfileResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhotoUrl = user.PhotoUrl,
            Phone = user.PhoneNumber,
            UserName = user.UserName,
            UserTypeId = user.UserTypeId
        };
    }

    public async Task<AuthModelResponseDto> ChangeUserPasswordAsync(int userId, ChangePasswordRequestDto requestDto)
    {
        var user = await FindUserByIdAsync(userId);

        var isCorrectPassword = await _authService.IsCorrectUserPasswordAsync(userId, requestDto.OldPassword);
        if (!isCorrectPassword)
        {
            throw new CustomExceptionWithStatusCode(400, "Your Password is not correct !!");
        }

        return await _authService.ResetClientPasswordAsync(new ResetPasswordDto(user.Email!), requestDto.NewPassword);
    }
}