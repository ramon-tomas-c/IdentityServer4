using MediatR;
using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Application.PasswordManagement.RecoveryPassword
{
    /// <summary>
    /// Command for password recovery
    /// </summary>
    public class Command : IRequest<OperationResult>
    {
        private Command(string username)
        {
            Username = username;
        }

        public static Command New(string username)
            => new Command(username);

        public string Username { get; private set; }
    }
}
