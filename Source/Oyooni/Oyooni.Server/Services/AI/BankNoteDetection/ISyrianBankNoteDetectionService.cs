﻿using Oyooni.Server.Enumerations;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.BankNoteDetection
{
    /// <summary>
    /// Represents a syrian bank note recognizer service contract
    /// </summary>
    public interface ISyrianBankNoteDetectionService
    {
        /// <summary>
        /// Recognizes a syrian bank note from an image data
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the bank note from</param>
        Task<SyrianBankNoteTypes> DetectBankNoteAsync(string imagePath, CancellationToken token = default);
    }
}