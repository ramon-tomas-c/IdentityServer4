using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Infrastructure.Mailing
{
    /// <summary>
    /// Mailing Content
    /// </summary>
    public class MailContent
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
