using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application general services
    /// </summary>
    public class GeneralServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add the http context accessor
            services.AddHttpContextAccessor();

            // Add mediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add signalR
            services.AddSignalR();

            // Add localization
            services.AddLocalization();

            // Return the services collection
            return services;
        }
    }
}
