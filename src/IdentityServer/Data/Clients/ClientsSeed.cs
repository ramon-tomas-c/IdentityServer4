using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Data.Clients
{
    /// <summary>
    /// Seeding class in which default Client configurations are built
    /// </summary>
    public class ClientsSeed
    {

        private readonly ClientSettings _clientSettings;

        public ClientsSeed(ClientSettings clientSettings)
        {
            _clientSettings = clientSettings;
        }

        /// <summary>
        /// Retrieve Client configurations
        /// </summary>
        /// <returns>Client configurations</returns>
        public IEnumerable<IdentityServer4.Models.Client> GetClients()
        {
            var clientResult = new List<IdentityServer4.Models.Client>();
            foreach (var client in _clientSettings?.Clients)
            {
                clientResult.Add(new IdentityServer4.Models.Client()
                {
                    ClientId = client.ClientId,
                    ClientName = client.ClientName,
                    AllowedGrantTypes = client.AllowedGrantTypes,
                    RequirePkce = client.RequirePkce,
                    RequireClientSecret = client.RequireClientSecret,
                    AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
                    AllowedCorsOrigins = client.WebUrls,
                    AllowOfflineAccess = client.AllowOfflineAccess,
                    RedirectUris = BuildUrlsWithPaths(client.WebUrls, client.RedirectUris),
                    PostLogoutRedirectUris = BuildUrlsWithPaths(client.WebUrls, client.PostLogoutRedirectUris),
                    AllowedScopes = client.AllowedScopes
                });
            }
            return clientResult;
        }

        public List<string> BuildUrlsWithPaths(IEnumerable<string> baseUrls, IEnumerable<string> paths) =>
            baseUrls.SelectMany(url => paths.Select(path => $"{url}{path}")).ToList();        
    }
}
