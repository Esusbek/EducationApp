﻿using EducationApp.Shared.Configs;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class EmailProvider : Interfaces.IEmailProvider
    {
        private readonly MailAddress _from;
        private readonly SmtpClient _smtp;

        public EmailProvider(IOptions<SmtpConfig> config)
        {
            _from = new MailAddress(config.Value.Adress);
            _smtp = new SmtpClient(config.Value.Server, config.Value.Port)
            {
                Credentials = new NetworkCredential(config.Value.Login, config.Value.Password),
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