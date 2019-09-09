using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using IdentityServer.Application.PasswordManagement.RecoveryPassword;
using IdentityServer.Infrastructure.Mailing;
using IdentityServer.Locale;
using IdentityServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.Application
{
    public class RecoverPasswordCommandHandlerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<IMailService> _mailService;
        private readonly Mock<IOptions<AppSettings>> _appOptions;

        public RecoverPasswordCommandHandlerTest()
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
           
            _mailService = new Mock<IMailService>();
            _appOptions = new Mock<IOptions<AppSettings>>();
        }

        [Fact]
        public async Task Handle_return_true_if_user_is_valid()
        {
            var fakeApplicationUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.FirstName = "FakeUser")
                .With(u => u.Type = UserType.Internal)
                .Build();

            var fakeToken = Guid.NewGuid().ToString();

            var fakeRecoverCmd = Command.New(fakeApplicationUser.FirstName);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult(fakeApplicationUser));

            _userManager.Setup(userMgr => userMgr.GeneratePasswordResetTokenAsync(It.Is<ApplicationUser>(u => u.FirstName == fakeApplicationUser.FirstName)))
              .Returns(Task.FromResult(fakeToken));

            _appOptions.Setup(options => options.Value)
                .Returns(new AppSettings());

            var handler = new CommandHandler(_userManager.Object, _mailService.Object, _appOptions.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeRecoverCmd, cltToken);


            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_return_false_if_user_does_not_exist()
        {
            var fakeApplicationUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.FirstName = "FakeUser")
                .With(u => u.Type = UserType.Internal)
                .Build();

            var fakeToken = Guid.NewGuid().ToString();

            var fakeRecoverCmd = Command.New(fakeApplicationUser.FirstName);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
               .Returns(Task.FromResult((ApplicationUser)null));

            _userManager.Setup(userMgr => userMgr.GeneratePasswordResetTokenAsync(It.Is<ApplicationUser>(u => u.FirstName == fakeApplicationUser.FirstName)))
              .Returns(Task.FromResult(fakeToken));

            _appOptions.Setup(options => options.Value)
                .Returns(new AppSettings());

            var handler = new CommandHandler(_userManager.Object, _mailService.Object, _appOptions.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeRecoverCmd, cltToken);


            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeFalse();
            result.Errors.First().Code.Should().Be(nameof(CommonErrors.ErrorUserDoesNotExist));
        }

        [Fact]
        public async Task Handle_return_false_if_user_is_external()
        {
            var fakeApplicationUser = Builder<ApplicationUser>.CreateNew()
                .With(u => u.FirstName = "FakeUser")
                .With(u => u.Type = UserType.External)
                .Build();

            var fakeToken = Guid.NewGuid().ToString();

            var fakeRecoverCmd = Command.New(fakeApplicationUser.FirstName);

            _userManager.Setup(userMgr => userMgr.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeApplicationUser));

            _userManager.Setup(userMgr => userMgr.GeneratePasswordResetTokenAsync(It.Is<ApplicationUser>(u => u.FirstName == fakeApplicationUser.FirstName)))
              .Returns(Task.FromResult(fakeToken));

            _appOptions.Setup(options => options.Value)
                .Returns(new AppSettings());

            var handler = new CommandHandler(_userManager.Object, _mailService.Object, _appOptions.Object);
            var cltToken = new System.Threading.CancellationToken();
            var result = await handler.Handle(fakeRecoverCmd, cltToken);


            result.Should().NotBeNull();
            result.Should().BeOfType<OperationResult>();
            result.Succeeded.Should().BeFalse();
            result.Errors.First().Code.Should().Be(nameof(CommonErrors.ErrorExternalUsersNotAllowed));
        }

        [Fact]
        public void ResetPassword_generator_should_create_correct_link()
        {
            var fakeUser = "FakeUser";
            var fakeToken = "123T65!adfr";
            var fakeBaseUrl = "http://myhost:5000";
            var fakePath = "/reset";
            var expectedResult = string.Format("http://myhost:5000/reset?token={0}&username={1}", fakeToken, fakeUser);

            var link = ResetPasswordLinkGenerator.Generate(fakeUser, fakeToken, fakeBaseUrl, fakePath);

            link.Should().Be(expectedResult);          
        }
    }
}
