using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.DigitRecognition
{
    /// <summary>
    /// Represents a digit recognizer contract
    /// </summary>
    public interface IDigitRecognizer
    {
        /// <summary>
        /// Recognizes the digit in an image file
        /// </summary>
        /// <param name="imagePath">Image file path to which the recognition will operate on</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        Task<int> RecognizeDigitFromImage(string imagePath, CancellationToken token = default);

        /// <summary>
        /// Recongizer the digit in the image data
        /// </summary>
        /// <param name="base64ImageData">Images bytes encoded in base64 format</param>
        /// <returns>An awaitable task containing the recognized digit value</returns>
        Task<int> RecognizeDigitFromImageData(string base64ImageData, CancellationToken token = default);
    }
}
