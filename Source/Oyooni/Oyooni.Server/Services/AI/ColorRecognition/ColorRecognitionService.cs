using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.ColorRecognition
{
    /// <summary>
    /// Represents a color recognizer service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IColorRecognitionService))]
    public class ColorRecognitionService : IColorRecognitionService
    {
        /// <summary>
        /// The http client used to call the local server serving the color detection
        /// </summary>
        protected readonly HttpClient _client;

        /// <summary>
        /// Constructrs a new instance of the <see cref="ColorRecognitionService"/> class using the passed parameters
        /// </summary>
        public ColorRecognitionService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClients.ColorRecognizerClient);
        }

        /// <summary>
        /// Recognizes colors in an image using the base64 representation of the image
        /// </summary>
        /// <param name="base64Data">Images bytes encoded in base64 format</param>
        /// <param name="k">Number of colors to detect</param>
        /// <returns>A dictionary where the keys are the detected color names and the values are the ratios of the corresponding color</returns>
        public async Task<Dictionary<string, float>> RecognizeColorsInImageDataAsync(string base64Data, int k = 3, CancellationToken token = default)
        {
            // Send a request to the local server with the image data and the number of colors to detect
            var response = await _client.PostAsync("/recognize-colors",
                new StringContent(JsonSerializer.Serialize(new
                {
                    Base64Data = base64Data,
                    K = k < 1 ? 3 : k
                }), Encoding.UTF8, "application/json"), token);

            // Parse the response
            var rootJObject = JObject.Parse(await response.Content.ReadAsStringAsync(token));

            // Create the dictionary to be returned
            var dictionary = new Dictionary<string, float>();

            // Loop over each pair
            foreach (var pair in rootJObject)
            {
                // Add to the dicionary
                dictionary.Add(pair.Key, pair.Value.Value<float>());
            }

            // Return the dictionary
            return dictionary;
        }
    }
}
