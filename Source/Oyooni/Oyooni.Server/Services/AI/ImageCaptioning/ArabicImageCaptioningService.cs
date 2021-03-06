using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.General;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.ImageCaptioning
{
    /// <summary>
    /// Represents an arabic image captioning service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IImageCaptioningService), ignoreForNow: false)]
    public class ArabicImageCaptioningService : IImageCaptioningService
    {
        /// <summary>
        /// The http client used to talk to the image captioning service
        /// </summary>
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// The network service
        /// </summary>
        protected readonly INetworkService _networkService;

        /// <summary>
        /// Constructs a new instance of the <see cref="SyrianBankNoteDetectionService"/> class using the passed parameters
        /// </summary>
        public ArabicImageCaptioningService(IHttpClientFactory httpClientFactory, INetworkService networkService)
            => (_httpClient, _networkService) 
            = (httpClientFactory.CreateClient(HttpClients.ImageCaptioningClient), networkService);

        /// <summary>
        /// Captions an image using the passed image path
        /// </summary>
        /// <param name="imagePath">Image path</param>
        /// <returns>Image caption</returns>
        public async Task<string> CaptionImageAsync(string imagePath, CancellationToken token = default)
        {
            if (!_networkService.IsPortInUse(_httpClient.BaseAddress.Port))
                throw new ServiceUnavailableException(Responses.General.ServiceUnavailable);

            // Call the image captioning service
            var result = await _httpClient.PostAsync("/caption-image",
                new StringContent(JsonSerializer.Serialize(new
                {
                    ImagePath = imagePath
                }), Encoding.UTF8, "application/json"), token);

            // Parse the response
            var rootJObject = JObject.Parse(await result.Content.ReadAsStringAsync(token));

            // Get the result value
            var caption = rootJObject["caption"].Value<string>();

            return caption;
        }
    }
}
