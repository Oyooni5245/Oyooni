using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using Oyooni.Server.Enumerations;
using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.BankNoteDetection
{
    /// <summary>
    /// Represents a syrian bank note recognizer service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IBankNoteDetectionService), ignoreForNow: false)]
    public class SyrianBankNoteDetectionService : IBankNoteDetectionService
    {
        /// <summary>
        /// The http client used to talk to the banknote detector service
        /// </summary>
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Constructs a new instance of the <see cref="SyrianBankNoteDetectionService"/> class using the passed parameters
        /// </summary>
        public SyrianBankNoteDetectionService(IHttpClientFactory httpClientFactory)
            => _httpClient = httpClientFactory.CreateClient(HttpClients.BankNoteDetectorClient);

        /// <summary>
        /// Recognizes a syrian bank note from an image data
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the bank note from</param>
        public async Task<SyrianBankNoteTypes> DetectBankNoteAsync(string imagePath, CancellationToken token = default)
        {
            // Call the banknote detector service
            var result = await _httpClient.PostAsync("/detect-banknote",
                new StringContent(JsonSerializer.Serialize(new
                {
                    ImagePath = imagePath
                }), Encoding.UTF8, "application/json"), token);

            // Parse the response
            var rootJObject = JObject.Parse(await result.Content.ReadAsStringAsync(token));

            // Get the result value
            var resultValue = rootJObject["result"].Value<int>();

            // If the result value is not defined then just return an undefined result
            if (!Enum.IsDefined(typeof(SyrianBankNoteTypes), resultValue))
                return SyrianBankNoteTypes.Undefined;

            // Return the result
            return (SyrianBankNoteTypes)resultValue;
        }
    }
}
