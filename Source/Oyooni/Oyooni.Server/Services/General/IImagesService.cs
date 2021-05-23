using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.General
{
    /// <summary>
    /// Represents an image service contract
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the base64 data of the image bytes
        /// </summary>
        /// <param name="imageFile">The image file containing the image data</param>
        /// <returns>Base64 representation of the image data</returns>
        Task<string> GetBase64ImageDataAsync(IFormFile imageFile, CancellationToken token = default);
    }
}
