namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a text recognition response dto
    /// </summary>
    public class TextRecognitionDto
    {
        /// <summary>
        /// The recognized text
        /// </summary>
        public string[] RecognizedText { get; set; }

        /// <summary>
        /// The brand name or the main text recognized
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextRecognitionDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="TextRecognitionDto"/> using the passed parameters
        /// </summary>
        public TextRecognitionDto(string[] recognizedText, string brandName)
        {
            RecognizedText = recognizedText;
            BrandName = brandName;
        }
    }
}
