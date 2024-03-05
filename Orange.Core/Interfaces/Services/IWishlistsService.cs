using Orange_Bay.DTOs.Programs;

namespace Orange_Bay.Interfaces.Services;

public interface IWishlistsService
{
    Task<bool> AddProgramToUserWishlistsAsync(int userId, int programId);
    Task<List<ProgramResponseDto>> GetWishlistsForUserAsync(int userId);
}