using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EducationApp.Shared.Configs;

namespace EducationApp.BusinessLogicLayer.Helpers
{
    public class EmailHelper
    {
        private readonly MailAddress _from;
        private readonly SmtpClient _smtp;

        public EmailHelper(SmtpConfig config)
        {
            _from = new MailAddress(config.Adress);
            _smtp = new SmtpClient(config.Server, config.Port);
            _smtp.Credentials = new NetworkCredential(config.Login, config.Password);
            _smtp.EnableSsl = true;
        }

        public async Task SendEmailAsync(MailAddress to, string subject, string body)
        {
            var msg = new MailMessage(_from, to);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            await _smtp.SendMailAsync(msg);
        }
    }
}
