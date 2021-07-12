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

        /// <summary>
        /// Create a temp file of the passed form file and returned its absolute path
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="token"></param>
        /// <returns>The absolute path of the newly created temp file</returns>
        public async Task<string> GetTempFileOfImage(IFormFile imageFile, CancellationToken token = default)
        {
            // Create a temp file and change its extension to the passed image file extension
            var tempFile = Path.GetTempFileName();
            var tempFilePath = Path.ChangeExtension(tempFile, Path.GetExtension(imageFile.FileName));

            File.Delete(tempFile);
            
            // Create a memory stream
            using (var memoryStream = new MemoryStream())
            {
                // Copy the image data to the memory stream
                await imageFile.CopyToAsync(memoryStream, token);
                using (var stream = File.Create(tempFilePath))
                {
                    // Get the image data as bytes
                    var data = memoryStream.ToArray();

                    // Copy the data to the temp file
                    await stream.WriteAsync(data.AsMemory(0, data.Length), token);
                }
            }

            // Return the temp file
            return tempFilePath;
        }
    }
}
