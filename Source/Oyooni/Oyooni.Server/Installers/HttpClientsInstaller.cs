using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Constants;
using System;
using Polly;
using Oyooni.Server.Services.General;

namespace Oyooni.Server.Installers
{
    public class HttpClientsInstaller : IInstaller
    {
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var bankNoteDetectionport = Convert.ToInt32(configuration["AIServicesEndpoints:BankNoteDetectionPort"]);
            var colorRecognitionPort = Convert.ToInt32(configuration["AIServicesEndpoints:ColorRecognitionPort"]);
            var imageCaptioningPort = Convert.ToInt32(configuration["AIServicesEndpoints:ImageCaptioningPort"]);
            var textRecognitionPort = Convert.ToInt32(configuration["AIServicesEndpoints:TextRecognitionPort"]);
            var localIp = "127.0.0.1";

            // Add Digit Recognition client
            services.AddHttpClient(HttpClients.BankNoteDetectorClient, (serviceProvider, client) =>
            {
                //var localIp = serviceProvider.GetService<INetworkService>().GetLocalIp();
                client.BaseAddress = new Uri($"http://{localIp}:{bankNoteDetectionport}");
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Color Recognition client
            services.AddHttpClient(HttpClients.ColorRecognizerClient, (serviceProvider, client) =>
            {
                //var localIp = serviceProvider.GetService<INetworkService>().GetLocalIp();
                client.BaseAddress = new Uri($"http://{localIp}:{colorRecognitionPort}");
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Add Arabic image captioning client
            services.AddHttpClient(HttpClients.ImageCaptioningClient, (serviceProvider, client) =>
            {
                //var localIp = serviceProvider.GetService<INetworkService>().GetLocalIp();
                client.BaseAddress = new Uri($"http://{localIp}:{imageCaptioningPort}");
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddHttpClient(HttpClients.TextRecognitionClient, (serviceProvider, client) =>
            {
                //var localIp = serviceProvider.GetService<INetworkService>().GetLocalIp();
                client.BaseAddress = new Uri($"http://{localIp}:{textRecognitionPort}");
            }).AddTransientHttpErrorPolicy(builder => builder
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            // Return the sevices collection
            return services;
        }
    }
}
