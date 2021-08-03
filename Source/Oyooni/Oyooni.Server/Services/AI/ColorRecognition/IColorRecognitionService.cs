using Oyooni.Server.Enumerations;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.ColorRecognition
{
    /// <summary>
    /// Represents a color recognizer service contract
    /// </summary>
    public interface IColorRecognitionService
    {
        /// <summary>
        /// Recognizes colors in an image using the passed image path
        /// </summary>
        /// <param name="imagePath">The image path to recognize the color in</param>
        /// <returns>The recongized dominant image</returns>
        Task<string> RecognizeColorInImageAsync(string imagePath, CancellationToken token = default);
    }
}
