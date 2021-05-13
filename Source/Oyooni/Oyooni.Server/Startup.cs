using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oyooni.Server.Extensions;
using Oyooni.Server.Hubs;
using Oyooni.Server.Middlewares;

namespace Oyooni.Server
{
    /// <summary>
    /// Represents the initialization class used to configure the services and the request pipeline
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Startup"/> class
        /// </summary>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Configures the services collection by added application-required services
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Run all dependenceis installers
            services.RunDependenciesInstallers(Configuration, WebHostEnvironment);
        }

        /// <summary>
        /// Configures the request pipeline by using several middlewares and handlers
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            // If we are in the development environment
            if (WebHostEnvironment.IsDevelopment())
            {
                // Use detailed exception page
                app.UseDeveloperExceptionPage();

                // Use swagger
                app.UseSwagger();

                // Set swagger options
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Oyooni.Server v1"));
            }

            // Use global exception handling middleware
            app.UseMiddleware<ExceptionsHandlingMiddleware>();

            // Use http redirection
            app.UseHttpsRedirection();

            // Use routing
            app.UseRouting();

            // Use cors
            app.UseCors("DefaultCorsPolicy");

            // Use Authentication
            app.UseAuthentication();

            // Use Authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Map controllers according to the Routes
                endpoints.MapControllers();

                // Map the SignalR hub
                endpoints.MapHub<ApplicationHub>("/hub");
            });
        }
    }
}
