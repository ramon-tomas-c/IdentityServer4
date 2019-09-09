using IdentityServer.Locale;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models.Account
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessageResourceName = nameof(CommonErrors.RequiredField), ErrorMessageResourceType = typeof(CommonErrors))]
        [EmailAddress(ErrorMessageResourceName = nameof(CommonErrors.Email), ErrorMessageResourceType = typeof(CommonErrors))]
        [Display(Name = nameof(PasswordRecovery.Email), ResourceType = typeof(PasswordRecovery))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonErrors.RequiredField), ErrorMessageResourceType = typeof(CommonErrors))]
        [MaxLength(100, ErrorMessageResourceName = nameof(CommonErrors.MaxLength), ErrorMessageResourceType = typeof(CommonErrors))]
        [Display(Name = nameof(PasswordRecovery.Password), ResourceType = typeof(PasswordRecovery))]
        public string Password { get; set; }

        [MaxLength(100, ErrorMessageResourceName = nameof(CommonErrors.MaxLength), ErrorMessageResourceType = typeof(CommonErrors))]
        [Display(Name = nameof(PasswordRecovery.PasswordCheck), ResourceType = typeof(PasswordRecovery))]
        [Compare(nameof(Password),
            ErrorMessageResourceName = nameof(CommonErrors.PasswordNotMatch),
            ErrorMessageResourceType = typeof(CommonErrors))]
        public string PasswordCheck { get; set; }
        public string Token { get; set; }
    }
}
