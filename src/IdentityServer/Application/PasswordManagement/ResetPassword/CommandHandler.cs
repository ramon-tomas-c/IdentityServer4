using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Locale;
using IdentityServer.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Application.PasswordManagement.ResetPassword
{
    /// <summary>
    /// Command handler which resets the password
    /// </summary>
    public class CommandHandler : IRequestHandler<Command, OperationResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return ResetPassword(request);
        }

        private async Task<OperationResult> ResetPassword(Command resetPasswordRequest)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordRequest.Username);
            if (user == null)
            {
                return OperationResult.Error(
                    new ValidationError(nameof(CommonErrors.ErrorUserDoesNotExist), string.Format(CommonErrors.ErrorUserDoesNotExist, resetPasswordRequest.Username)));
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.ResetPasswordToken, resetPasswordRequest.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new ValidationError(error.Code, error.Description));

                return OperationResult.Error(errors);
            }

            return OperationResult.Success();
        }
    }
}