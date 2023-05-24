using System.Net;
using System.Net.Mail;
using IdentityUI.Core.Core.OptionsModel;
using Microsoft.Extensions.Options;

namespace IdentityUI.Core.Service.Services
{

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host= _emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Identity Core | Şifre Sıfırlama Linki";
            mailMessage.Body = @$"
                <h4>Şifrenizi Yenilemek İçin Aşağıdaki Linke Tıklayınız</h4>
                <p><a href='{resetEmailLink}'>Şifre Yenileme Linki</a></p>";
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
