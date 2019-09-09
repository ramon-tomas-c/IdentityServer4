using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Settings;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Authentication extension to register multiple Azure AD 
        /// external providers from configuration. 
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddAADProvidersFromConfig(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {
            var azureADProviders = configuration.GetSection("AAD").Get<List<AzureADOptions>>();

            azureADProviders?.ForEach(provider => 
            {
                authenticationBuilder
                    .AddOpenIdConnect(provider.ProviderName, provider.ProviderName, options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                        options.Authority = provider.Authority;
                        options.ClientId = provider.ClientId;
                        options.ResponseType = provider.ResponseType;
                        options.CallbackPath = provider.CallbackPath;
                        options.SignedOutCallbackPath = provider.SignedOutCallbackPath;
                        options.RemoteSignOutPath = provider.RemoteSignOutPath;
                        options.RequireHttpsMetadata = provider.RequireHttpsMetadata;
                        options.GetClaimsFromUserInfoEndpoint = provider.GetClaimsFromUserInfoEndpoint;
                    });
            });
            
            return authenticationBuilder;
        }

        /// <summary>
        /// Authentication extension to register an Azure AD 
        /// external provider. 
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="azureADOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddAADProvider(this AuthenticationBuilder authenticationBuilder, Action<AzureADOptions> azureADOptions)
        {
            var provider = new AzureADOptions();
            azureADOptions(provider);

            authenticationBuilder
                .AddOpenIdConnect(provider.ProviderName, provider.ProviderName, options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = provider.Authority;
                    options.ClientId = provider.ClientId;
                    options.ResponseType = provider.ResponseType;
                    options.CallbackPath = provider.CallbackPath;
                    options.SignedOutCallbackPath = provider.SignedOutCallbackPath;
                    options.RemoteSignOutPath = provider.RemoteSignOutPath;
                    options.RequireHttpsMetadata = provider.RequireHttpsMetadata;
                    options.GetClaimsFromUserInfoEndpoint = provider.GetClaimsFromUserInfoEndpoint;
                });

            return authenticationBuilder;
        }
    }
}
