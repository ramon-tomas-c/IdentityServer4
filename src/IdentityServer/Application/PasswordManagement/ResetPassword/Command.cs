using MediatR;
using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Application.PasswordManagement.ResetPassword
{
    /// <summary>
    /// Command for password reset
    /// </summary>
    public class Command : IRequest<OperationResult>
    {
        private Command(string username, string password, string resetPasswordToken)
        {
            Username = username;
            Password = password;
            ResetPasswordToken = resetPasswordToken;
        }

        public static Command New(string username, string password, string resetPasswordToken)
            => new Command(username, password, resetPasswordToken);

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ResetPasswordToken { get; private set; }
    }
}
