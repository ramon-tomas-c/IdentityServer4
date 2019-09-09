using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Data
{
    /// <summary>
    /// Provides a default seed for IS resource 
    /// configurations
    /// </summary>
    public class ConfigurationDbContextSeed
    {
        /// <summary>
        /// Applies the IS resource configuration seed
        /// </summary>
        /// <param name="context">
        /// DbContext for the IdentityServer configuration data
        /// </param>
        /// <param name="env">App environment</param>
        /// <param name="logger">App logger</param>
        /// <param name="settings">App settings</param>
        /// <param name="clientSettings">Client settings</param>
        /// <returns></returns>
        public async Task EnsureSeedData(ConfigurationDbContext context, IHostingEnvironment env,
            ILogger<ConfigurationDbContext> logger, 
            IOptions<AppSettings> settings,
            IOptions<ClientSettings> clientSettings)
        {
            var appSettings = settings.Value;
            var clientUrlSettings = clientSettings.Value;

            Console.WriteLine("Seeding database...");

            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients(clientUrlSettings).ToList())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApis().ToList())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}