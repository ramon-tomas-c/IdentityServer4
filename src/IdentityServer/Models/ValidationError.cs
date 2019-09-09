using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    /// <summary>
    /// Model for validation errors
    /// </summary>
    public class ValidationError
    {
        public string Code { get; set; }
        public string Reason { get; set; }

        public ValidationError() { }

        public ValidationError(string code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }
}
