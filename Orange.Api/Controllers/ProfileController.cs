using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Profile;
using Orange_Bay.Interfaces.Services;
using Orange.EF.Services;
using Microsoft.AspNetCore.Identity;
using Orange_Bay.Models.Auth;

namespace Orange.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController
{
    private readonly IProfilesService _profilesService;

    public ProfileController(IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _profilesService = new ProfilesService(authService, userManager);
    }

    [HttpGet("Client")]
    public async Task<IActionResult> GetUserProfileAsync( int userId)
    {
        var result = await _profilesService.GetProfileForUserAsync(userId);
        var response = ResponseModelDto<ProfileResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPut("Client/UpdateProfile")]
    public async Task<IActionResult> UpdateUserProfileAsync( UpdateProfileDto requestDto)
    {
        var result = await _profilesService.UpdateUserProfileAsync(requestDto);
        var response = ResponseModelDto<ProfileResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPut("Client/ChangePassword")]
    public async Task<IActionResult> ChangeUserPasswordAsync( int userId , ChangePasswordRequestDto requestDto)
    {
        var result = await _profilesService.ChangeUserPasswordAsync(userId, requestDto);
        var response = ResponseModelDto<AuthModelResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }
}