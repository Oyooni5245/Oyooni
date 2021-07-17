using Oyooni.Server.Enumerations;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a colors recognition result data transfer object
    /// </summary>
    public class ColorRecognitionResultDto
    {
        /// <summary>
        /// The recognized colors
        /// </summary>
        public RecognizedColor RecognizedColor { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ColorRecognitionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ColorRecognitionResultDto"/> class using the passed parameters
        /// </summary>
        public ColorRecognitionResultDto(RecognizedColor recognizedColor) => RecognizedColor = recognizedColor;
    }
}
