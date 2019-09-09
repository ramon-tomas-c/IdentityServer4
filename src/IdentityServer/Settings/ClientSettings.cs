using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    /// <summary>
    /// Client Settings
    /// </summary>
    public class ClientSettings
    {
        public IEnumerable<ClientOptions> Clients { get; set; }
    }

    /// <summary>
    /// Client Urls
    /// </summary>
    public class ClientOptions
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public ICollection<string> AllowedGrantTypes { get; set; } = GrantTypes.Code;
        public bool RequirePkce { get; set; } = true;
        public bool RequireClientSecret { get; set; } = false;
        public bool AllowAccessTokensViaBrowser { get; set; } = true;
        public bool AllowOfflineAccess { get; set; } = false;
        public ICollection<string> AllowedCorsOrigins { get; set; }
        public ICollection<string> WebUrls { get; set; }
        public ICollection<string> RedirectUris { get; set; }
        public ICollection<string> PostLogoutRedirectUris { get; set; }
        public ICollection<string> AllowedScopes { get; set; }

    }
}
