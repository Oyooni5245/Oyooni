using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using Oyooni.Server.Constants;
using Oyooni.Server.Services.AI.VAQ;
using System;
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
        /// Captions an image according the to passed language and image data
        /// </summary>
        /// <param name="language">Language to caption the image to</param>
        /// <param name="base64ImageData">Image data in base64 string</param>
        /// <returns>Image caption</returns>
        public Task<string> CaptionImageAsync(CaptioningLanguage language, string base64ImageData, CancellationToken token = default)
        {
            return Task.FromResult(language switch
            {
                CaptioningLanguage.Arabic => "وصف بسيط عن الصورة",
                CaptioningLanguage.English => "Some caption of the image",
                _ => throw new Exception("Unsupported lanuage")
            });
        }
    }
}
