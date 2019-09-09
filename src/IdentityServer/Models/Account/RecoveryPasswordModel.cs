using IdentityServer.Locale;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models.Account
{
    public class RecoveryPasswordModel
    {
        [Required(ErrorMessageResourceName = nameof(CommonErrors.RequiredField), ErrorMessageResourceType = typeof(CommonErrors))]
        [EmailAddress(ErrorMessageResourceName = nameof(CommonErrors.Email), ErrorMessageResourceType = typeof(CommonErrors))]
        [Display(Name = nameof(PasswordRecovery.Email), ResourceType = typeof(PasswordRecovery))]
        public string Username { get; set; }
    }
}
