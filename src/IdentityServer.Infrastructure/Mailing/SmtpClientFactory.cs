using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace IdentityServer.Infrastructure.Mailing
{
    /// <summary>
    /// Factory to generate the Smtp client
    /// </summary>
    public class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly IOptions<Mailing> _options;
        private MailingSmtp SmptOptions => _options.Value.Smtp;
        public SmtpClientFactory(IOptions<Mailing> options)
        {
            _options = options;
        }

        public SmtpClient Build()
        {
            return new SmtpClient(SmptOptions.Host, SmptOptions.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(SmptOptions.User, SmptOptions.Password)
            };
        }
    }
}
