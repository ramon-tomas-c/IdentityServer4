using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityServer.Attributes;
using IdentityServer.Locale;
using IdentityServer.Models;
using IdentityServer.Models.Account;
using System.Threading.Tasks;

namespace IdentityServer.Controllers.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class PasswordManagementController : Controller
    {
        private readonly IMediator _mediator;

        public PasswordManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("recovery")]
        public IActionResult RecoveryPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            return View(nameof(RecoveryPassword));
        }

        [HttpPost("recovery")]
        public async Task<IActionResult> RecoveryPassword([FromForm]RecoveryPasswordModel request)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            if (!ModelState.IsValid)
            {
                return View(nameof(RecoveryPassword), request);
            }

            var result = await _mediator.Send(Application.PasswordManagement.RecoveryPassword.Command.New(request.Username));
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(nameof(RecoveryPassword), request);

            }
            return View("RecoveryPasswordEmailSent");
        }


        [Route("reset")]
        [HttpGet]
        public IActionResult ResetPassword([FromQuery]string username, [FromQuery]string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError(
                    nameof(CommonErrors.ErrorInvalidNameOrToken),
                    string.Format(CommonErrors.ErrorInvalidNameOrToken, username, token));
            }

            var model = new ResetPasswordModel
            {
                Username = username,
                Token = token
            };
            return View(nameof(ResetPassword), model);
        }

        [Route("reset")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordModel request)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(nameof(ResetPassword), request);
            }

            var result = await _mediator.Send(Application.PasswordManagement.ResetPassword.Command.New(request.Username, request.Password, request.Token));
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(nameof(ResetPassword), request);
            }

            TempData["UserFeedback"] = PasswordRecovery.ResetPasswordComplete;

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        private void AddErrors(OperationResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Reason);
            }
        }
    }
}