using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Mailing
{
    public interface IMailService
    {
        Task SendMail(MailContent mailContent);
    }
}
