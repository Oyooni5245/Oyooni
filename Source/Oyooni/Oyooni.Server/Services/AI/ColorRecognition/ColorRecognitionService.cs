using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using Oyooni.Server.Enumerations;
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
        /// Recognizes colors in an image using the passed image path
        /// </summary>
        /// <param name="imagePath">The image path to recognize the color in</param>
        /// <returns>The recongized dominant image</returns>
        public async Task<RecognizedColor> RecognizeColorInImageAsync(string imagePath, CancellationToken token = default)
        {
            // Send a request to the local server with the image path
            var response = await _client.PostAsync("/recognize-color",
                new StringContent(JsonSerializer.Serialize(new
                {
                    ImagePath = imagePath,
                }), Encoding.UTF8, "application/json"), token);

            // Parse the response
            var rootJObject = JObject.Parse(await response.Content.ReadAsStringAsync(token));

            // Return the dictionary
            return (RecognizedColor)rootJObject["recognizedColor"].Value<int>();
        }
    }
}
