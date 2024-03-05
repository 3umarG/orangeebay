using System.Net.Sockets;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Security;
using Serilog;

namespace Orange.EF.Services;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmail(ApplicationUser user, string newPassword)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail)
            };
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = $"Reset Password for : {user.Email} Orange Bay Account";
            var builder = new BodyBuilder
            {
                TextBody = $"Dear {user.FullName},\n\n" +
                           $"Your password has been reset. Your new temporary password is:   {newPassword}\n\n" +
                           "Please login and change your password as soon as possible."
            };


            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            Log.Information($"Finishing Sending Email to {user.Email}");
        }
        catch (SocketException e)
        {
            Log.Error(e.Message);
            throw new CustomExceptionWithStatusCode(400,
                "There is an error while sending Email to you, please check internet connection then try again !!");
        }
    }
}