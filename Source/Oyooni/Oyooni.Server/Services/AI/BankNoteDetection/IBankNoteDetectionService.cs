using Oyooni.Server.Enumerations;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.BankNoteDetection
{
    /// <summary>
    /// Represents a bank note recognizer service contract
    /// </summary>
    public interface IBankNoteDetectionService
    {
        /// <summary>
        /// Recognizes a bank note from the passed image
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the bank note from</param>
        Task<SyrianBankNoteTypes> DetectBankNoteAsync(string imagePath, CancellationToken token = default);
    }
}
