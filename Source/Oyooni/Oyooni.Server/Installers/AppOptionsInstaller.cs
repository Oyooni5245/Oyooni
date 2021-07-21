using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Options;
using System;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application options
    /// </summary>
    public class AppOptionsInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add the auth token options
            services.AddSingleton(sp =>
            {
                // Get the bearer settings section
                var bearerSettings = configuration.GetSection("BearerSettings");

                // Return a new auth token options with the configurations
                return new AuthTokenOptions(
                    bearerSettings["Issuer"], bearerSettings["Audience"],
                    bearerSettings["Key"], bearerSettings["TokenExpiryTime"],
                    bearerSettings["RefreshTokenExpiryTime"]);
            });

            // Add the paths options
            services.AddSingleton(sp => new PathsOptions(sp.GetService<IWebHostEnvironment>(),
                sp.GetService<IHttpContextAccessor>()));

            // Return the services collections
            return services;
        }
    }
}
