using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.ImageCaptioning
{
    /// <summary>
    /// Represents an mock image captioning service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IImageCaptioningService), ignoreForNow: false)]
    public class MockImageCaptioningService : IImageCaptioningService
    {
        /// <summary>
        /// Captions an image using the image data
        /// </summary>
        /// <param name="base64ImageData">Image data in base64 string</param>
        /// <returns>Image caption</returns>
        public Task<string> CaptionImageAsync(string base64ImageData, CancellationToken token = default)
        {
            return Task.FromResult("وصف بسيط عن الصورة");
        }
    }
}
