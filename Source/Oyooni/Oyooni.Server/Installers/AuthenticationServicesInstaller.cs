using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oyooni.Server.Extensions;
using System.Threading.Tasks;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application authentication services
    /// </summary>
    public class AuthenticationServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Create the validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["BearerSettings:Audience"],
                ValidIssuer = configuration["BearerSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(configuration["BearerSettings:Key"].ToUTF8Bytes()),
            };

            // Add the validation parameters as singleton
            services.AddSingleton(tokenValidationParameters);

            // Add authentication
            services.AddAuthentication(options =>
            {
                // Set forbid, authenticate, and sign in schemes to JwtBearer
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // Configure the events of the jwt authentication
                options.Events = new JwtBearerEvents
                {
                    // When a message is recieved
                    OnMessageReceived = (context) =>
                    {
                        // Get the token from qurey parameters
                        var accessToken = context.Request.Query["access_token"];

                        // Get the path of the request
                        var path = context.Request.Path;

                        // If the token is not empty and the request indeed is for the hub
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                            // Set the access token
                            context.Token = accessToken;

                        // Return a completed task
                        return Task.CompletedTask;
                    }
                };

                // Let it use the validation parameters created earlier
                options.TokenValidationParameters = tokenValidationParameters;
            });

            // Return the services collection
            return services;
        }
    }
}
