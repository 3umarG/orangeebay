using Orange_Bay.DTOs.Programs;
using Orange_Bay.DTOs.Programs.Request;
using Orange_Bay.DTOs.Shared;

namespace Orange_Bay.Interfaces.Services;

public interface IProgramsService
{
    Task<List<ProgramResponseDto>> GetAllProgramsByUserTypeAsync(int? userId, DateTime? dateTime);
    Task<ProgramOverviewResponseDto> GetProgramOverviewAsync(int programId, int? userTypeId);
    Task<List<ProgramPlanResponseDto>> GetProgramPlansAsync(int programId);
    Task<PaginatedResponseDto<ProgramReviewResponseDto>> GetProgramReviewsByIdAsync(int programId, int page);
    Task<ProgramReviewResponseDto> AddProgramReviewAsync(ProgramReviewRequestDto dto);
    Task<List<string>> AddProgramImagesAsync(int programId, ImagesRequestDto requestDto);
    Task<ProgramIncludedAndExcludedDetails> GetProgramIncludedAndExcludedDetails(int programId);
}
