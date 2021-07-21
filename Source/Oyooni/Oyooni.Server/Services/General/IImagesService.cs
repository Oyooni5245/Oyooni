using Microsoft.AspNetCore.Http;
using Oyooni.Server.Common;
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

        /// <summary>
        /// Create a temp file of the passed form file and returned its absolute path
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="token"></param>
        /// <returns>The absolute path of the newly created temp file</returns>
        Task<DisposableTempFile> GetTempFileOfImage(IFormFile imageFile, CancellationToken token = default);
    }
}
