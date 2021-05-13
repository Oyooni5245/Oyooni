using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application cors policy
    /// </summary>
    public class CorsPolicyServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add cors
            services.AddCors(options =>
            {
                // Add the default policy
                options.AddPolicy("DefaultCorsPolicy", builder =>
                {
                    // Accept all origins, headers, and methods
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Return the services collection
            return services;
        }
    }
}
