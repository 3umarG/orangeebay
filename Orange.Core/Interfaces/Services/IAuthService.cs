using Orange_Bay.DTOs.Auth;
using Orange_Bay.Models.Auth;

namespace Orange_Bay.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthModelResponseDto> RegisterAsync(UserRegisterDto dto, string role = "user");

        public Task<AuthModelResponseDto> LoginAsync(UserLoginDto dto);

        public Task<AuthModelResponseDto>
            ResetClientPasswordAsync(ResetPasswordDto dto); // Overload for reset password and send email

        public Task<AuthModelResponseDto>
            ResetClientPasswordAsync(ResetPasswordDto dto, string newPassword); // Overload for change password

        public Task<ApplicationUser> FindUserByIdAsync(int id);
        public Task<int> FindUserTypeIdByUserAsync(int userId);
        public Task<bool> AnyUserWithIdAsync(int id);

        public Task<bool> IsCorrectUserPasswordAsync(int userId, string password);
    }

    public record ResetPasswordWithEmailDto(
        string Email
    );
}