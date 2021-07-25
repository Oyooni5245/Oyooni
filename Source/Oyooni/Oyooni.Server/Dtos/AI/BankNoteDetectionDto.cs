using Oyooni.Server.Enumerations;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a banknote detection response dto
    /// </summary>
    public class BankNoteDetectionDto
    {
        /// <summary>
        /// The bank note that has been recognized
        /// </summary>
        public SyrianBankNoteTypes BankNoteType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BankNoteDetectionDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BanknoteDetectionResponse"/> using the passed parameters
        /// </summary>
        public BankNoteDetectionDto(SyrianBankNoteTypes bankNoteType)
        {
            BankNoteType = bankNoteType;
        }
    }
}
