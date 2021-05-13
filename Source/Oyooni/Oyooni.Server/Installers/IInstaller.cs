using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oyooni.Server.Installers
{
    public interface IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment);
    }
}
