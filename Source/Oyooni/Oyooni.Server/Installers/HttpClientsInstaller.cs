using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Constants;
using System;
using Polly;

namespace Oyooni.Server.Installers
{
    public class HttpClientsInstaller : IInstaller
    {
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add Digit Recognition client
            services.AddHttpClient(HttpClients.DigitRecognizerClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:DigitRecognitionEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Color Recognition client
            services.AddHttpClient(HttpClients.ColorRecognizerClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:ColorRecognitionEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Arabic image captioning client
            services.AddHttpClient(HttpClients.ArabicImageCaptioningClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:ArabicImageCaptioningEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddHttpClient(HttpClients.EnglishImageCaptioningClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:EnglishImageCaptioningEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add English VQA client
            services.AddHttpClient(HttpClients.EnglishVAQClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:EnglishVAQEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Return the sevices collection
            return services;
        }
    }
}
