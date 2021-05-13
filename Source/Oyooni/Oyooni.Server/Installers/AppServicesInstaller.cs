using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System.Linq;
using System.Reflection;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application services
    /// </summary>
    public class AppServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Get all classes that have the Injected attribute
            var injectedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(InjectedAttribute)).Any()).Select(t => new
                {
                    Type = t,
                    InjectedAttribute = t.GetCustomAttribute<InjectedAttribute>()
                });

            // Loop over all class
            foreach (var injectedType in injectedTypes)
            {
                // Check the lifetiem and register the service accordingly
                switch (injectedType.InjectedAttribute.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                    default:
                        services.AddTransient(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                }
            }

            // Return the services collection
            return services;
        }
    }
}
