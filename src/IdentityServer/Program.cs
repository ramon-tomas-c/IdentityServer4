// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.Configuration;
using IdentityServer.Settings;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build()
                .MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
                .MigrateDbContext<ApplicationDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                    var settings = services.GetService<IOptions<AppSettings>>();
                    
                    if(settings.Value.SeedData)
                    {
                        new ApplicationDbContextSeed()
                            .EnsureSeedAsync(context, env, logger, settings)
                            .Wait();
                    }                    
                })
                .MigrateDbContext<ConfigurationDbContext>((context,services)=> 
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ConfigurationDbContext>>();
                    var settings = services.GetService<IOptions<AppSettings>>();
                    var clientSettings = services.GetService<IOptions<ClientSettings>>();

                    if (settings.Value.SeedData)
                    {
                        new ConfigurationDbContextSeed()
                            .EnsureSeedData(context, env, logger, settings, clientSettings)
                            .Wait();
                    }                       
                })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        if (!context.HostingEnvironment.IsDevelopment())
                        {
                            var builtConfig = config.Build();
                            var vaultSettings = new Vault();
                            builtConfig.GetSection(nameof(Vault)).Bind(vaultSettings);
                            var tokenProvider = new AzureServiceTokenProvider();
                            var kvClient = new KeyVaultClient((authority, resource, scope) => tokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                            config.AddAzureKeyVault($"https://{vaultSettings.Name}.vault.azure.net/", kvClient, new DefaultKeyVaultSecretManager());                           
                        }                        
                    })
                    .UseSerilog((context, configuration) =>
                    {
                        configuration
                            .ReadFrom.Configuration(context.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.File(@"identityserver4_log.txt")
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                            .WriteTo.ApplicationInsights(context.Configuration["Appinsights_IK"], TelemetryConverter.Traces);
                    });
        }
    }
}