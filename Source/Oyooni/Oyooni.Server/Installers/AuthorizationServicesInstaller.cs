using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Data;
using Oyooni.Server.Data.BusinessModels;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application authorization services
    /// </summary>
    public class AuthorizationServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add authorization
            services.AddAuthorization()
                // Add identity user type
                .AddIdentityCore<AppUser>(options =>
                {
                    // Password options
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = true;

                    // SignIn options
                    options.SignIn.RequireConfirmedAccount = false;

                    // User options
                    options.User.RequireUniqueEmail = true;
                })
                // Add roles type
                .AddRoles<IdentityRole>()
                // Add ef core stores
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // Add token providers
                .AddDefaultTokenProviders();

            // Return the services collection
            return services;
        }
    }
}
