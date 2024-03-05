using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Programs;
using Orange_Bay.Interfaces.Services;
using Orange.EF;
using Orange.EF.Services;

namespace Orange.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WishlistsController : ControllerBase
{
    private readonly IWishlistsService _wishlistsService;

    public WishlistsController(ApplicationDbContext context, IAuthService authService)
    {
        _wishlistsService = new WishlistsService(context, authService);
    }


    [HttpPost]
    public async Task<IActionResult> AddOrRemoveProgramToWishlistsAsync(
         int userId,
         int programId
    )
    {
        var result = await _wishlistsService.AddProgramToUserWishlistsAsync(userId, programId);
        var response = ResponseModelDto<bool>.BuildSuccessResponse(result);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgramsInUserWishlistsAsync([FromHeader(Name = "uid")] int userId)
    {
        var result = await _wishlistsService.GetWishlistsForUserAsync(userId);
        var response = ResponseModelDto<List<ProgramResponseDto>>.BuildSuccessResponse(result);
        return Ok(response);
    }
}