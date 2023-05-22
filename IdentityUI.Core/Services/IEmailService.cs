namespace IdentityUI.Core.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetEmailLink, string toEmail);
    }
}
