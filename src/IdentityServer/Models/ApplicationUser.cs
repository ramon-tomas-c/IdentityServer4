using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    /// <summary>
    /// Represents the user identity
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Disabled { get; set; }
        public UserType Type { get; set; }
    }

    /// <summary>
    /// Type of user depending on whether they come from
    /// Internal or external providers
    /// </summary>
    public enum UserType
    {
        Internal,
        External
    }
}
