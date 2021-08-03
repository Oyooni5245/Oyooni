using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.TextRecogntion
{
    /// <summary>
    /// Represents a text recognition service contract
    /// </summary>
    public interface ITextRecognitionService
    {
        /// <summary>
        /// Recognizes the text in an image
        /// </summary>
        /// <param name="imagePath">The image path</param>
        Task<(string, string[], string, string, string)> RecognizeTextAsync(string imagePath, bool isDocument = false, CancellationToken token = default);
    }
}
