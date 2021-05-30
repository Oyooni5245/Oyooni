using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.TextRecogntion
{
    /// <summary>
    /// Represents a text recogntion service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(ITextRecognitionService), ignoreForNow: false)]
    public class MockTextRecognitionService : ITextRecognitionService
    {
        /// <summary>
        /// Recognizes the text in an image
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the text in</param>
        public Task<string> RecognizeTextAsync(string base64ImageData, CancellationToken token = default)
        {
            return Task.FromResult("Some awesome text that is superly recognized from the image");
        }
    }
}
