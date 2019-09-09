using FizzWare.NBuilder;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using IdentityServer.Controllers;
using IdentityServer.Controllers.Account;
using IdentityServer.Models;
using IdentityServer.Models.Account;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.Controllers
{
    public class PasswordManagementControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<HttpContext> _fakeContext;

        public PasswordManagementControllerTests()
        {
            _mediator = new Mock<IMediator>();

            _fakeContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            _fakeContext.Setup(x => x.User).Returns(principal);
        }

        [Fact]
        public void Recovery_request_should_redirect_to_Login_if_user_authenticated()
        {                        
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = passwordMgntController.RecoveryPassword();

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be(nameof(AccountController.Login));
            redirectToActionResult.ControllerName.Should().Be("Account");
        }

        [Fact]
        public void Recovery_request_should_return_recovery_password_page_if_user_not_authenticated()
        {            
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = passwordMgntController.RecoveryPassword();

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be(nameof(passwordMgntController.RecoveryPassword));
        }

        [Fact]
        public async Task Recovery_post_should_redirect_to_Login_if_user_authenticated()
        {
            var data = Builder<RecoveryPasswordModel>.CreateNew()
                .With(d => d.Username = "FakeUserName")
                .Build();
           
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = await passwordMgntController.RecoveryPassword(data);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be(nameof(AccountController.Login));
            redirectToActionResult.ControllerName.Should().Be("Account");
        }

        [Fact]
        public async Task Recovery_post_should_return_RecoveryPasswordEmailSent_page_if_email_sent()
        {
            var data = Builder<RecoveryPasswordModel>.CreateNew()
                .With(d => d.Username = "FakeUserName")
                .Build();

            var operationResult = Builder<OperationResult>.CreateNew()
                .With(o => o.Succeeded = true)
                .Build();
           
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            _mediator.Setup(mediatr => mediatr.Send(It.IsAny<IdentityServer.Application.PasswordManagement.RecoveryPassword.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationResult);

            var result = await passwordMgntController.RecoveryPassword(data);

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be("RecoveryPasswordEmailSent");
        }

        [Fact]
        public void ResetPassword_request_should_redirect_to_Login_if_user_authenticated()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "153F7H!f";
            
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = passwordMgntController.ResetPassword(fakeUser, fakeToken);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be(nameof(AccountController.Login));
            redirectToActionResult.ControllerName.Should().Be("Account");
        }

        [Fact]
        public void ResetPassword_request_should_return_to_ResetPassword_page_if_succeeded()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "153F7H!f";
            
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = passwordMgntController.ResetPassword(fakeUser, fakeToken);

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be(nameof(passwordMgntController.ResetPassword));
        }

        [Fact]
        public async Task ResetPassword_post_should_redirect_to_Login_if_user_authenticated()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "153F7H!f";
            var fakeUserPwd = "Pass@word";

            var data = Builder<ResetPasswordModel>.CreateNew()
                .With(m => m.Username = fakeUser)
                .With(m => m.Token = fakeToken)
                .With(m => m.Password = fakeUserPwd)
                .Build();
           
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = await passwordMgntController.ResetPassword(data);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be(nameof(AccountController.Login));
            redirectToActionResult.ControllerName.Should().Be("Account");
        }

        [Fact]
        public async Task ResetPassword_post_should_redirect_to_Login_if_succeeded()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "153F7H!f";
            var fakeUserPwd = "Pass@word";

            var operationResult = Builder<OperationResult>.CreateNew()
                .With(o => o.Succeeded = true)
                .Build();

            var data = Builder<ResetPasswordModel>.CreateNew()
                .With(m => m.Username = fakeUser)
                .With(m => m.Token = fakeToken)
                .With(m => m.Password = fakeUserPwd)
                .Build();
           
            var tempData = new TempDataDictionary(_fakeContext.Object, Mock.Of<ITempDataProvider>());
            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);            

            _mediator.Setup(mediatr => mediatr.Send(It.IsAny<IdentityServer.Application.PasswordManagement.ResetPassword.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationResult);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;
            passwordMgntController.TempData = tempData;

            var result = await passwordMgntController.ResetPassword(data);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be(nameof(AccountController.Login));
            redirectToActionResult.ControllerName.Should().Be("Account");
        }

        [Fact]
        public async Task ResetPassword_post_should_return_to_ResetPassword_page_if_failed()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "153F7H!f";
            var fakeUserPwd = "Pass@word";

            var operationResult = Builder<OperationResult>.CreateNew()
                .With(o => o.Succeeded = false)
                .Build();

            var data = Builder<ResetPasswordModel>.CreateNew()
                .With(m => m.Username = fakeUser)
                .With(m => m.Token = fakeToken)
                .With(m => m.Password = fakeUserPwd)
                .Build();

            _fakeContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            _mediator.Setup(mediatr => mediatr.Send(It.IsAny<IdentityServer.Application.PasswordManagement.ResetPassword.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(operationResult);

            var passwordMgntController = new PasswordManagementController(_mediator.Object);
            passwordMgntController.ControllerContext.HttpContext = _fakeContext.Object;

            var result = await passwordMgntController.ResetPassword(data);

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewName.Should().Be(nameof(passwordMgntController.ResetPassword));
        }        
    }
}
