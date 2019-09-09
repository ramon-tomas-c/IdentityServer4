using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    /// <summary>
    /// Provides additional identity information about users
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        /// <summary>
        /// Load claims for a user
        /// </summary>
        /// <param name="context">
        /// Models the request for user claims and return those claims
        /// </param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            context.IssuedClaims = claims;
        }

        /// <summary>
        ///  Indicate if a user is currently allowed to obtain tokens
        /// </summary>
        /// <param name="context">
        /// Models the request to determine is the user is currently allowed 
        /// to obtain tokens
        /// </param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = user != null && !user.Disabled;
        }
    }
}
