using EducationApp.Shared.Configs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Helpers
{
    public class EmailProvider
    {
        private readonly MailAddress _from;
        private readonly SmtpClient _smtp;

        public EmailProvider(SmtpConfig config)
        {
            _from = new MailAddress(config.Adress);
            _smtp = new SmtpClient(config.Server, config.Port)
            {
                Credentials = new NetworkCredential(config.Login, config.Password),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(MailAddress to, string subject, string body)
        {
            var msg = new MailMessage(_from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            await _smtp.SendMailAsync(msg);
        }
    }
}
