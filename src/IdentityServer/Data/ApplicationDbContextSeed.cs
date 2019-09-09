using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    /// <summary>
    /// Provides a default seed for user identities
    /// </summary>
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        
        /// <summary>
        /// Applies the identity seed
        /// </summary>
        /// <param name="context">Identity context</param>
        /// <param name="env">App Environment</param>
        /// <param name="logger">App logger</param>
        /// <param name="settings">App settings</param>
        /// <returns></returns>
        public async Task EnsureSeedAsync(ApplicationDbContext context, IHostingEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings)
        {
            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(GetDefaultUser());
                await context.SaveChangesAsync();
            }  
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
            new ApplicationUser()
            {
                Email = "demouser@domain.com",
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567890",
                UserName = "demouser@domain.com",
                NormalizedEmail = "DEMOUSER@DOMAIN.COM",
                NormalizedUserName = "DEMOUSER@DOMAIN.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"), 
                Type = UserType.Internal,
                FirstName = "demouser",
                LastName = "demouser"
            };
            
            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            return new List<ApplicationUser>()
            {
                user
            };
        }
    }
}