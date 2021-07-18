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
        /// Captions an image using the passed image path
        /// </summary>
        /// <param name="imagePath">Image path</param>
        /// <returns>Image caption</returns>
        Task<string> CaptionImageAsync(string imagePath, CancellationToken token = default);
    }
}
