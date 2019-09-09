namespace IdentityServer.Settings
{
    /// <summary>
    /// Azure AD external provider options
    /// </summary>
    public class AzureADOptions
    {
        /// <summary>
        /// The response type. The default is id_token.
        /// </summary>
        public string ResponseType { get; set; } = "id_token";
        /// <summary>
        /// The request path within the application's base path 
        /// where the user will be returned. The default is /signin-aad.
        /// </summary>
        public string CallbackPath { get; set; } = "/signin-aad";
        /// <summary>
        /// The request path within the application's base 
        /// path where the user will be returned after sign out.
        /// The default is /signout-callback-aad.
        /// </summary>
        public string SignedOutCallbackPath { get; set; } = "/signout-callback-aad";
        /// <summary>
        /// Requests received on this path will cause the middleware 
        /// to invoke SignOut. The default is /signout-aad.
        /// </summary>
        public string RemoteSignOutPath { get; set; } = "/signout-aad";
        /// <summary>
        /// Allows to retrieve additional claims after 
        /// creating the identity. The default is true.
        /// </summary>
        public bool GetClaimsFromUserInfoEndpoint { get; set; } = true;
        /// <summary>
        /// Https metadata requirement for the metadata 
        /// address or authority. The default is true.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;
        /// <summary>
        /// Name of the Azure AD provider
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// Azure AD issuing authority
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// Application ID in Azure AD
        /// </summary>
        public string ClientId { get; set; }
    }
}
