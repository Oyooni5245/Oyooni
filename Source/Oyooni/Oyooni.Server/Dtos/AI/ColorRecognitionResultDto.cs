using System.Collections.Generic;

namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a colors recognition result data transfer object
    /// </summary>
    public class ColorRecognitionResultDto
    {
        /// <summary>
        /// The key-value pairs that represent the recognized colors and their corresponding ratios
        /// </summary>
        public Dictionary<string, float> RecognizedColors { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ColorRecognitionResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ColorRecognitionResultDto"/> class using the passed parameters
        /// </summary>
        public ColorRecognitionResultDto(Dictionary<string, float> recognizedColors) => RecognizedColors = recognizedColors;
    }
}
