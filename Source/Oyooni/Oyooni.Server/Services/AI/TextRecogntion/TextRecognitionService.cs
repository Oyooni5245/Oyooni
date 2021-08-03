using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.General;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.TextRecogntion
{
    /// <summary>
    /// Represents a text recognition service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(ITextRecognitionService), ignoreForNow: false)]
    public class TextRecognitionService : ITextRecognitionService
    {
        /// <summary>
        /// The http client used to talk to the text recognizer service
        /// </summary>
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// The network service
        /// </summary>
        protected readonly INetworkService _networkService;

        /// <summary>
        /// Constructs a new instance of the <see cref="TextRecognitionService"/> class using the passed parameters
        /// </summary>
        public TextRecognitionService(IHttpClientFactory httpClientFactory, INetworkService networkService)
            => (_httpClient, _networkService)
            = (httpClientFactory.CreateClient(HttpClients.TextRecognitionClient), networkService);

        /// <summary>
        /// Recognizes the text in an image
        /// </summary>
        /// <param name="imagePath">The image path</param>
        public async Task<(string, string[], string, string, string)> RecognizeTextAsync(string imagePath, bool isDocument = false, CancellationToken token = default)
        {
            if (!_networkService.IsPortInUse(_httpClient.BaseAddress.Port))
                throw new ServiceUnavailableException(Responses.General.ServiceUnavailable);

            // Call the text recognizer service
            var result = await _httpClient.PostAsync("/recognize-text",
                new StringContent(JsonSerializer.Serialize(new
                {
                    ImagePath = imagePath,
                    IsDocument = isDocument
                }), Encoding.UTF8, "application/json"), token);

            // Parse the response
            var rootJObject = JObject.Parse(await result.Content.ReadAsStringAsync(token));

            var brandName = string.Empty;
            var language = rootJObject["language"].ToObject<string>();
            var fullText = new List<string>();
            var arabicText = string.Empty;
            var englishText = string.Empty;

            if (rootJObject.ContainsKey("texts"))
            {
                brandName = rootJObject["brand_name"].Value<string>();
                var textsNode = rootJObject["texts"];
                englishText = textsNode["en"].ToObject<string>();
                arabicText = textsNode["ar"].ToObject<string>();

                if (textsNode["full_text"].Type == JTokenType.Array)
                    fullText = textsNode["full_text"].ToObject<List<string>>();
                else if (textsNode["full_text"].Type == JTokenType.String)
                {
                    var subText = textsNode["full_text"].ToObject<string>();
                    fullText.Add(subText);
                }
            }
            else
            {
                if (rootJObject["text"].Type == JTokenType.Array)
                    fullText = rootJObject["text"].ToObject<List<string>>();
                else if (rootJObject["text"].Type == JTokenType.String)
                {
                    var subText = rootJObject["text"].ToObject<string>();
                    fullText.Add(subText);
                }
            }


            // return the result
            return (brandName, fullText.ToArray(), language, englishText, arabicText);
        }
    }
}
