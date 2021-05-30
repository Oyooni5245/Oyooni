using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.TextRecogntion
{
    /// <summary>
    /// Represents a text recogntion service contract
    /// </summary>
    public interface ITextRecognitionService
    {
        /// <summary>
        /// Recognizes the text in an image
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the text in</param>
        Task<string> RecognizeTextAsync(string base64ImageData, CancellationToken token = default);
    }
}
