using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request to recognize the colors in an image file
    /// </summary>
    public class RecognizeColorRequest
    {
        /// <summary>
        /// The image file to recognize the colors in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// The number of colors to detect in the image file
        /// </summary>
        public int NumberOfColorsToDetect { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeColorRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RecognizeColorRequest"/> class using the passed parameters
        /// </summary>
        public RecognizeColorRequest(IFormFile file, int numberOfColorsToDetect)
            => (File, NumberOfColorsToDetect) = (file, numberOfColorsToDetect);
    }
}
