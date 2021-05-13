using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Middlewares;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application middlewares
    /// </summary>
    public class MiddlewaresInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add exception handling middleware
            services.AddTransient<ExceptionsHandlingMiddleware>();

            // Return the services collection
            return services;
        }
    }
}
