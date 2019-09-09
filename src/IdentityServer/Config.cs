// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using IdentityServer.Data.Clients;
using IdentityServer.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer
{
    /// <summary>
    /// Provides access to the different resource configurations
    /// </summary>
    public static class Config
    {
        private static IEnumerable<string> AccessTokenRequiredClaims = new[] {
            JwtClaimTypes.Subject,
            JwtClaimTypes.Name,
            JwtClaimTypes.GivenName,
            JwtClaimTypes.FamilyName,
            JwtClaimTypes.Email,
            CustomClaimTypes.UserType,
            ClaimTypes.NameIdentifier,
            ClaimTypes.Name,
            ClaimTypes.Upn
        };


        /// <summary>
        /// Retrieves Identity resource configurations
        /// </summary>
        /// <returns>
        /// Identity resource configurations
        /// </returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// Retrieves Api resource configurations
        /// </summary>
        /// <returns>
        /// Api resource configurations
        /// </returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("api", "SlaterAndGordon RTA API", AccessTokenRequiredClaims),
            };
        }

        /// <summary>
        /// Retrieves Client resource configurations
        /// </summary>
        /// <param name="urls">
        /// Client Urls to be mapped
        /// </param>
        /// <returns>
        /// Client resource configurations
        /// </returns>
        public static IEnumerable<IdentityServer4.Models.Client> GetClients(ClientSettings clientSettings)
        {
            return new ClientsSeed(clientSettings)
                .GetClients();
        }
    }
}
