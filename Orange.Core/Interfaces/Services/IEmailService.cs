using Orange_Bay.Models.Auth;

namespace Orange_Bay.Interfaces.Services;

public interface IEmailService
{
    public Task SendEmail(ApplicationUser user,string newPassword);
}