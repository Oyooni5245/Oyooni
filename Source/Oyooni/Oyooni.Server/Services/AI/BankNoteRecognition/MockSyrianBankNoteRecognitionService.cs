using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using Oyooni.Server.Enumerations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.BankNoteRecognition
{
    /// <summary>
    /// Represents a syrian bank note recognizer service
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(ISyrianBankNoteRecognitionService), ignoreForNow: false)]
    public class MockSyrianBankNoteRecognitionService : ISyrianBankNoteRecognitionService
    {
        /// <summary>
        /// Recognizes a syrian bank note from an image data
        /// </summary>
        /// <param name="base64ImageData">The image data to recognize the bank note from</param>
        public Task<SyrianBankNoteTypes> RecognizeBankNoteAsync(string base64ImageData, CancellationToken token = default)
        {
            var values = Enum.GetValues(typeof(SyrianBankNoteTypes));

            return Task.FromResult((SyrianBankNoteTypes)values.GetValue(new Random().Next(values.Length)));
        }
    }
}
