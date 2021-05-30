using System.Collections.Generic;
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
        /// Recognizes colors in an image using the base64 representation of the image
        /// </summary>
        /// <param name="base64Data">Images bytes encoded in base64 format</param>
        /// /// <param name="k">Number of colors to detect</param>
        /// <returns>A dictionary where the keys are the detected color names and the values are the ratios of the corresponding color</returns>
        Task<Dictionary<string, float>> RecognizeColorsInImageDataAsync(string base64Data, int k = 3, CancellationToken token = default);
    }
}
