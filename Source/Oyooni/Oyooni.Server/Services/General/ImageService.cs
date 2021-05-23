using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.General
{
    /// <summary>
    /// Represents an image service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IImageService))]
    public class ImageService : IImageService
    {
        /// <summary>
        /// Gets the base64 data of the image bytes
        /// </summary>
        /// <param name="imageFile">The image file containing the image data</param>
        /// <returns>Base64 representation of the image data</returns>
        public async Task<string> GetBase64ImageDataAsync(IFormFile imageFile, CancellationToken token = default)
        {
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream, token);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
