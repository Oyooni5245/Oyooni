using Oyooni.Server.Enumerations;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a bank note recognition result data transfer object
    /// </summary>
    public class BankNoteDetectionResultDto
    {
        /// <summary>
        /// The digit value that has been recognized
        /// </summary>
        public SyrianBankNoteTypes BankNoteType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BankNoteDetectionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BankNoteDetectionResultDto"/> class using the passed parameters
        /// </summary>
        public BankNoteDetectionResultDto(SyrianBankNoteTypes bankNoteType) => BankNoteType = bankNoteType;
    }
}
