namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a text recognition result data transfer object
    /// </summary>
    public class TextRecognitionResultDto
    {
        /// <summary>
        /// The recognized text
        /// </summary>
        public string RecognizedText { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextRecognitionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="TextRecognitionResultDto"/> class using the passed parameters
        /// </summary>
        public TextRecognitionResultDto(string recognizedText) => RecognizedText = recognizedText;
    }
}
