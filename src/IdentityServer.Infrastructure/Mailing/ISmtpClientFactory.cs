using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace IdentityServer.Infrastructure.Mailing
{
    public interface ISmtpClientFactory
    {
        SmtpClient Build();
    }
}
