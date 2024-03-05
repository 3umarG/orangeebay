using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.Interfaces.Services;

namespace Orange.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    public AuthController(IAuthService authService, IEmailService emailService)
    {
        _authService = authService;
        _emailService = emailService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromForm] UserRegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        var response = new ResponseModelDto<AuthModelResponseDto>(true, null, 200, result);
        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(UserLoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        var response = new ResponseModelDto<AuthModelResponseDto>(true, null, 200, result);
        return Ok(response);
    }


    [HttpPut("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await _authService.ResetClientPasswordAsync(dto);
        var response = new ResponseModelDto<AuthModelResponseDto>(true, null, 200, result);
        return Ok(response);
    }


    // [HttpPost("SendEmail")]
    // public async Task<IActionResult> SendEmailAsync(ResetPasswordWithEmailDto dto)
    // {
    //     await _emailService.SendEmail(dto.Email);
    //     return Ok();
    // }
}