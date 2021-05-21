using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.DigitRecognition
{
    /// <summary>
    /// Represents a mock digit recognizer service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IDigitRecognizer), ignoreForNow: false)]
    public class DigitRecognizer : IDigitRecognizer
    {
        protected readonly HttpClient _client;

        public DigitRecognizer(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClients.DigitRecognizerClient);
        }

        /// <summary>
        /// Recognizes the digit in an image file
        /// </summary>
        /// <param name="imagePath">Image file path to which the recognition will operate on</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        public Task<int> RecognizeDigitFromImage(string imagePath, CancellationToken token = default)
        {
            // Return a random number between 0 and 11 (excluding 10)
            // TODO: Add recognition through image file
            return Task.FromResult(new Random().Next(10));
        }

        /// <summary>
        /// Recongizer the digit in the image data
        /// </summary>
        /// <param name="base64ImageData">Images bytes encoded in base64 format</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        public async Task<int> RecognizeDigitFromImageData(string base64ImageData, CancellationToken token = default)
        {
            // Send a request to the local server with the image data
            var response = await _client.PostAsync("/recognize-digit",
                new StringContent(JsonSerializer.Serialize(new 
                    { Base64Data = base64ImageData }), Encoding.UTF8, "application/json"), token);

            // Parse the response and return the predicted digit
            return JObject.Parse(await response.Content.ReadAsStringAsync(token))["recognizedDigit"].Value<int>();
        }
    }
}
