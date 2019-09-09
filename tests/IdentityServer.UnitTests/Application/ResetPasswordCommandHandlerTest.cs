using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using IdentityServer.Application.PasswordManagement.ResetPassword;
using IdentityServer.Infrastructure.Mailing;
using IdentityServer.Locale;
using IdentityServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.Application
{
    public class ResetPasswordCommandHandlerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;

        public ResetPasswordCommandHandlerTest()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>(
                   new Mock<IUserStore<ApplicationUser>>().Object,
                   new Mock<IOptions<IdentityOptions>>().Object,
                   new Mock<IPasswordHasher<ApplicationUser>>().Object,
                   new IUserValidator<ApplicationUser>[0],
                   new IPasswordValidator<ApplicationUser>[0],
                   new Mock<ILookupNormalizer>().Object,
                   new Mock<IdentityErrorDescriber>().Object,
                   new Mock<IServiceProvider>().Object,
                   new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        [Fact]
        public async Task Handle_return_true_if_reset_password_succeeded()
        {
            var fakeUserName = "FakeUserName";
            var fakeUserPassword = "FakeUser@Password!";
            var fakeToken = Guid.NewGuid().ToString();

            var fakeApplicationUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.FirstName = "FakeName")
                .With(u => u.UserName = fakeUserName)
                .With(u => u.Type = UserType.Internal)
                .Build();

            var fakeResetCmd = Command.New(fakeUserName, fakeUserPassword, fakeToken);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult(fakeApplicationUser));

            _userManager.Setup(userMgr => userMgr.ResetPasswordAsync(fakeApplicationUser, fakeToken, fakeUserPassword))
              .Returns(Task.FromResult<IdentityResult>(new FakeIdentityResult(true)));

            var handler = new CommandHandler(_userManager.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeResetCmd, cltToken);

            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_return_false_if_reset_password_failed()
        {
            var fakeUserName = "FakeUserName";
            var fakeUserPassword = "FakeUser@Password!";
            var fakeToken = Guid.NewGuid().ToString();

            var fakeApplicationUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.FirstName = "FakeName")
                .With(u => u.UserName = fakeUserName)
                .With(u => u.Type = UserType.Internal)
                .Build();

            var fakeResetCmd = Command.New(fakeUserName, fakeUserPassword, fakeToken);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult(fakeApplicationUser));

            _userManager.Setup(userMgr => userMgr.ResetPasswordAsync(fakeApplicationUser, fakeToken, fakeUserPassword))
              .Returns(Task.FromResult<IdentityResult>(new FakeIdentityResult(false)));

            var handler = new CommandHandler(_userManager.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeResetCmd, cltToken);

            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeFalse();            
        }

        [Fact]
        public async Task Handle_return_false_if_user_does_not_exists()
        {
            var fakeUserName = "FakeUserName";
            var fakeUserPassword = "FakeUser@Password!";
            var fakeToken = Guid.NewGuid().ToString();

            var fakeResetCmd = Command.New(fakeUserName, fakeUserPassword, fakeToken);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult((ApplicationUser)null));

            var handler = new CommandHandler(_userManager.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeResetCmd, cltToken);

            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeFalse();
            result.Errors.First().Code.Should().Be(nameof(CommonErrors.ErrorUserDoesNotExist));
        }
    }

    class FakeIdentityResult : IdentityResult
    {
        public FakeIdentityResult(bool succeed)
        {
            Succeeded = succeed;
        }
    }
}
