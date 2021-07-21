using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Oyooni.Server.Filters;
using System.Reflection;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application controllers services
    /// </summary>
    public class ControllersServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add controllers
            services.AddControllers(options =>
            {
                // Enable routing
                options.EnableEndpointRouting = true;

                // Add the data validation filter
                options.Filters.Add<DataValidationFilter>();

            })
                // Configure the bahavior of the api as suppress the model state when invalid
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)

                // Add the fluent validation to be integration with the model state
                .AddFluentValidation(config =>
                {
                    // Register validators from the current assembly
                    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                    // Make Cascade mode as continue
                    config.ValidatorOptions.CascadeMode = CascadeMode.Continue;

                    // Run regular validation after fluent validation runs
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                });

            // Return the services collection
            return services;
        }
    }
}
