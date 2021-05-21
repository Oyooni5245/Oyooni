namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a digit recognition result data transfer object
    /// </summary>
    public class DigitRecognitionResultDto
    {
        /// <summary>
        /// The digit value that has been recognized
        /// </summary>
        public int RecognizedDigit { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DigitRecognitionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="DigitRecognitionResultDto"/> class using the passed parameters
        /// </summary>
        public DigitRecognitionResultDto(int recognizedDigit) => RecognizedDigit = recognizedDigit;
    }
}
