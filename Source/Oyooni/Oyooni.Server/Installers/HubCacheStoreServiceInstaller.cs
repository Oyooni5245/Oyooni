using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Enumerations;
using Oyooni.Server.Services.Cache;
using Oyooni.Server.Services.Cache.InMemory;
using Oyooni.Server.Services.Cache.Redis;
using ServiceStack.Redis;
using System;

namespace Oyooni.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the hub cache store service
    /// </summary>
    public class HubCacheStoreServiceInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var hubCacheTypeString = configuration["GeneralSettings:HubCacheType"];
            var hubCacheType = (HubCacheType)Convert.ToInt32(hubCacheTypeString);

            switch (hubCacheType)
            {
                case HubCacheType.InMemory:
                    services.AddSingleton<IHubCacheService, InMemoryHubCacheService>();
                    break;

                case HubCacheType.RedisCache:
                    // Create a redis settings instance
                    var redisSettings = new RedisSettings();

                    // Bind the redis settings section to the redis settings instance
                    configuration.GetSection(nameof(RedisSettings)).Bind(redisSettings);

                    // Add the redis settings to the DI container
                    services.AddSingleton(redisSettings);

                    var redisManagerPool = new RedisManagerPool(host: $"{redisSettings.Host}:{redisSettings.Port}");
                    
                    // Add the redis client to the DI container
                    services.AddSingleton<IRedisClientsManagerAsync>(_ => redisManagerPool);

                    // Add the redis cache service to the DI container
                    services.AddScoped<IHubCacheService, RedisCacheHubService>();

                    break;

                default:
                    throw new ArgumentException($"Value {hubCacheTypeString} is not defined");
            }

            // Return the services collection
            return services;
        }
    }
}
