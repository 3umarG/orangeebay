using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orange_Bay.DTOs.Auth;
using Orange_Bay.DTOs.Programs;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.Interfaces.Services;

namespace Orange.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProgramsController
{
    private readonly IProgramsService _programsService;
    private readonly ITokenService _tokenService;

    public ProgramsController(IAuthService authService, ITokenService tokenService, IProgramsService programsService)
    {
        _tokenService = tokenService;
        _programsService = programsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgramsByUserTypeId(int? userId, DateTime? date)
    {
        var result = await _programsService.GetAllProgramsByUserTypeAsync(userId, date);
        var response = new ResponseModelDto<List<ProgramResponseDto>>(true, null, 200, result);
        return new OkObjectResult(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProgramByIdAsync([FromHeader(Name = "uid")] int? userId,int id)
    {
        var result = await _programsService.GetProgramOverviewAsync(id,userId);
        var response = ResponseModelDto<ProgramOverviewResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpGet("{id:int}/Plan")]
    public async Task<IActionResult> GetProgramPlanByIdAsync(int id)
    {
        var result = await _programsService.GetProgramPlansAsync(id);
        var response = ResponseModelDto<List<ProgramPlanResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpGet("{id:int}/Reviews")]
    public async Task<IActionResult> GetProgramReviewsByIdAsync(int id, [FromQuery] int page = 0)
    {
        var result = await _programsService.GetProgramReviewsByIdAsync(id, page);
        var response = ResponseModelDto<PaginatedResponseDto<ProgramReviewResponseDto>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }


    [HttpPost("Reviews")]
    [Authorize]
    public async Task<IActionResult> AddProgramReviewAsync(ProgramReviewRequestDto requestDto)
    {
        var result = await _programsService.AddProgramReviewAsync(requestDto);
        var response = ResponseModelDto<ProgramReviewResponseDto>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpPost("{id:int}/Images")]
    public async Task<IActionResult> AddImageToProgramAsync(int id, [FromForm] ImagesRequestDto requestDto)
    {
        var result = await _programsService.AddProgramImagesAsync(id, requestDto);
        var response = ResponseModelDto<List<string>>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }

    [HttpGet("{id:int}/IncludedAndExcluded")]
    public async Task<IActionResult> GetProgramIncludedAndExcludedDetails(int id)
    {
        var result = await _programsService.GetProgramIncludedAndExcludedDetails(id);
        var response = ResponseModelDto<ProgramIncludedAndExcludedDetails>.BuildSuccessResponse(result);
        return new OkObjectResult(response);
    }
}