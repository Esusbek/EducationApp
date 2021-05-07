using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface IEmailProvider
    {
        public Task SendEmailAsync(MailAddress to, string subject, string body);
    }
}
