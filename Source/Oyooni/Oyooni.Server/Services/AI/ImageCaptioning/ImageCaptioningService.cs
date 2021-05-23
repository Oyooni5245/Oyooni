using Oyooni.Server.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.ImageCaptioning
{
    /// <summary>
    /// Represents an image captioning service contract
    /// </summary>
    public interface IImageCaptioningService
    {
        /// <summary>
        /// Captions an image according the to passed language and image data
        /// </summary>
        /// <param name="language">Language to caption the image to</param>
        /// <param name="base64ImageData">Image data in base64 string</param>
        /// <returns>Image caption</returns>
        Task<string> CaptionImageAsync(CaptioningLanguage language, string base64ImageData, CancellationToken token = default);
    }
}
