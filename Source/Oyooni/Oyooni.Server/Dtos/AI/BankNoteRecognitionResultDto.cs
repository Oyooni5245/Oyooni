using Oyooni.Server.Enumerations;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a bank note recognition result data transfer object
    /// </summary>
    public class BankNoteRecognitionResultDto
    {
        /// <summary>
        /// The digit value that has been recognized
        /// </summary>
        public SyrianBankNoteTypes BankNoteType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BankNoteRecognitionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BankNoteRecognitionResultDto"/> class using the passed parameters
        /// </summary>
        public BankNoteRecognitionResultDto(SyrianBankNoteTypes bankNoteType) => BankNoteType = bankNoteType;
    }
}
