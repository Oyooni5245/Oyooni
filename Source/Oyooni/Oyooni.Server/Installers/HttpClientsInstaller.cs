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
            services.AddHttpClient(HttpClients.BankNoteRecognizerClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:BankNoteRecognitionEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Color Recognition client
            services.AddHttpClient(HttpClients.ColorRecognizerClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:ColorRecognitionEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Arabic image captioning client
            services.AddHttpClient(HttpClients.ImageCaptioningClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:ImageCaptioningEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddHttpClient(HttpClients.TextRecognitionClient, client =>
            {
                client.BaseAddress = new Uri(configuration["AIServicesEndpoints:TextRecognitionEndpoint"]);
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Return the sevices collection
            return services;
        }
    }
}
