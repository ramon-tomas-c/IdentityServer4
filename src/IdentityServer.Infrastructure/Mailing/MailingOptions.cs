using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Infrastructure.Mailing
{
    /// <summary>
    /// Mailing options
    /// </summary>
    public class Mailing
    {
        public MailingSmtp Smtp { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }

    /// <summary>
    /// SMTP server options
    /// </summary>
    public class MailingSmtp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
