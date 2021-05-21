using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.DigitRecognition
{
    /// <summary>
    /// Represents a mock digit recognizer service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IDigitRecognizer), ignoreForNow: true)]
    public class MockDigitRecognizer : IDigitRecognizer
    {
        /// <summary>
        /// Mocks recognizing the digit in an image file
        /// </summary>
        /// <param name="imagePath">Image file path to which the recognition will operate on</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        public Task<int> RecognizeDigitFromImage(string imagePath, CancellationToken token = default)
        {
            // Return a random number between 0 and 11 (excluding 10)
            return Task.FromResult(new Random().Next(10));
        }

        /// <summary>
        /// Recongizer the digit in the image data
        /// </summary>
        /// <param name="base64ImageData">Images bytes encoded in base64 format</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        public Task<int> RecognizeDigitFromImageData(string base64ImageData, CancellationToken token = default)
        {
            // Return a random number between 0 and 11 (excluding 10)
            return Task.FromResult(new Random().Next(10));
        }
    }
}
