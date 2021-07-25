using Oyooni.Server.Enumerations;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a color recognition response dto
    /// </summary>
    public class ColorRecognitionDto
    {
        /// <summary>
        /// The recognized color
        /// </summary>
        public RecognizedColor RecognizedColor { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ColorRecognitionDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ColorRecognitionDto"/> using the passed parameters
        /// </summary>
        public ColorRecognitionDto(RecognizedColor recognizedColor)
        {
            RecognizedColor = recognizedColor;
        }
    }
}
