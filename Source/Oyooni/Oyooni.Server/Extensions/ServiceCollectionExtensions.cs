using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Installers;
using System;
using System.Linq;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> interface
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Runs all installers that implement <see cref="IInstaller"/> interface
        /// </summary>
        public static void RunDependenciesInstallers(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Get all types that implement the IInstaller interface and call the install dependencies method
            typeof(Startup).Assembly.GetExportedTypes()
                 .Where(type => typeof(IInstaller).IsAssignableFrom(type) &&
                         !type.IsInterface && !type.IsAbstract)
                 .Select(type => Activator.CreateInstance(type))
                 .Cast<IInstaller>().ToList()
                 .ForEach(installer =>
                 {
                     installer.InstallDependencies(services, configuration, webHostEnvironment);
                 });
        }
    }
}
