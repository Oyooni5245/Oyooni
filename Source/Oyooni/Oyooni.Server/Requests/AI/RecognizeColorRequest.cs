using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request to recognize the color in an image file
    /// </summary>
    public class RecognizeColorRequest
    {
        /// <summary>
        /// The image file to recognize the color in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeColorRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RecognizeColorRequest"/> class using the passed parameters
        /// </summary>
        public RecognizeColorRequest(IFormFile file) => (File) = (file);
    }
}
