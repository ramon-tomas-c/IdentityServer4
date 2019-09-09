using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Application.PasswordManagement.RecoveryPassword;
using IdentityServer.Infrastructure.Mailing;
using IdentityServer.Models;
using IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    /// <summary>
    /// IoC extension class
    /// </summary>
    public static class IoCExtensions
    {
        /// <summary>
        /// Register in IoC the different app services
        /// </summary>
        /// <param name="services">
        /// Service Collection
        /// </param>
        /// <param name="configuration"></param>
        /// <returns>
        /// Service Collection
        /// </returns>
        public static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterIdentityServices(services);
            RegisterCommands(services);
            RegisterMailingServices(services);
            RegisterSupportedCultures(services);

            return services;
        }

        private static void RegisterIdentityServices(IServiceCollection services)
        {
            services.AddTransient<IProfileService, ProfileService>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
        }

        private static void RegisterSupportedCultures(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var defaultCulture = new CultureInfo("es");
                var supportedCultures = new[]
                {
                    defaultCulture
                };

                options.DefaultRequestCulture = new RequestCulture(culture: defaultCulture, uiCulture: defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<ClientSettings>(configuration);
            services.Configure<Mailing>(configuration.GetSection(nameof(Mailing)));

            return services;
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddMediatR(typeof(Command).Assembly);
        }

        private static void RegisterMailingServices(IServiceCollection services)
        {
            services.AddTransient<ISmtpClientFactory, SmtpClientFactory>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient(c =>
            {
                var smtpClientFactory = c.GetRequiredService<ISmtpClientFactory>();
                return smtpClientFactory.Build();
            });
        }
    }
}
