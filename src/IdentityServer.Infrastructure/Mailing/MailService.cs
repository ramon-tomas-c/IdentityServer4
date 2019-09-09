using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Mailing
{
    /// <summary>
    /// Service to send mails
    /// </summary>
    public class MailService : IMailService
    {
        private readonly IOptions<Mailing> _options;
        private readonly SmtpClient _smtpClient;

        public MailService(IOptions<Mailing> options, SmtpClient smtpClient)
        {
            _options = options;
            _smtpClient = smtpClient;
        }

        /// <summary>
        /// Send mail method
        /// </summary>
        /// <param name="mailContent">Mail Content</param>
        /// <returns></returns>
        public async Task SendMail(MailContent mailContent)
        {
            var mailAddressFrom = new MailAddress(_options.Value.SenderEmail, _options.Value.SenderName);
            var mailAddressTo = new MailAddress(mailContent.EmailAddress);
            var mail = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = mailContent.Subject,
                Body = mailContent.Body,
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mail);
        }
    }
}
