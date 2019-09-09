using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using IdentityServer.Infrastructure.Mailing;
using IdentityServer.Locale;
using IdentityServer.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Application.PasswordManagement.RecoveryPassword
{
    /// <summary>
    /// Command handler which generates the email for password recovery
    /// </summary>
    public class CommandHandler : IRequestHandler<Command, OperationResult>
    {
        private const string ResetPasswordPath = "/reset";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly AppSettings _settings;

        public CommandHandler(
            UserManager<ApplicationUser> userManager,
            IMailService mailService,
            IOptions<AppSettings> options)
        {
            _userManager = userManager;
            _mailService = mailService;
            _settings = options.Value;
        }

        public Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return SendRecoveryPasswordEmail(request);
        }

        private async Task<OperationResult> SendRecoveryPasswordEmail(Command resetPasswordRequest)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordRequest.Username);
            if (user == null)
            {
                return OperationResult.Error(
                    new ValidationError(nameof(CommonErrors.ErrorUserDoesNotExist), string.Format(CommonErrors.ErrorUserDoesNotExist, resetPasswordRequest.Username)));
            }

            if (user.Type == UserType.External)
            {
                return OperationResult.Error(
                    new ValidationError(nameof(CommonErrors.ErrorExternalUsersNotAllowed), string.Format(CommonErrors.ErrorExternalUsersNotAllowed, user.UserName)));
            }

            var link = await GenerateResetPasswordLink(user, ResetPasswordPath);

            await _mailService.SendMail(new MailContent
            {
                EmailAddress = user.Email,                
                Subject = PasswordRecovery.ResetPasswordEmailSubject,
                Body = string.Format(PasswordRecovery.ResetPasswordEmailBody, link),
            });

            return OperationResult.Success();
        }
        private async Task<string> GenerateResetPasswordLink(ApplicationUser user, string path)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = ResetPasswordLinkGenerator.Generate(user.UserName, token, _settings.IdentityUrl, path);          
            return link;
        }

        private IQueryCollection GenerateResetPasswordQueryParams(string token, string username)
        {
            var queryParams = new QueryCollection(
                new Dictionary<string, StringValues>()
                {
                    {"token", token },
                    {"username", username }
                });
            return queryParams;
        }
    }
}
