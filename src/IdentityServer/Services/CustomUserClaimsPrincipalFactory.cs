using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    /// <summary>
    /// Extends UserClaimsPrincipalFactory class to add 
    /// additional custom claims to claims principal for 
    /// a given user
    /// </summary>
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        /// <summary>
        /// Creates a Claims Principal for a given user
        /// </summary>
        /// <param name="user">
        /// The user to create a Claims Principal from
        /// </param>
        /// <returns>
        /// Returns the generated Claims Principal
        /// </returns>
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            var identity = (ClaimsIdentity)principal.Identity;
            await AddClaims(user, identity);

            return principal;
        }

        private async Task AddClaims(ApplicationUser user, ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""));
            identity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.LastName ?? ""));

            if (user.Type == UserType.Internal)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
            }      

            var userType = ((int)user.Type).ToString();
            identity.AddClaim(new Claim(CustomClaimTypes.UserType, userType));
            await Task.CompletedTask;
        }
    }
}
