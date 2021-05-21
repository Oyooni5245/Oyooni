using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request to recognize the digit in an image file
    /// </summary>
    public class RecognizeDigitRequest
    {
        /// <summary>
        /// The image file to recognize the digit in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeDigitRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RecognizeDigitRequest"/> class using the passed parameters
        /// </summary>
        public RecognizeDigitRequest(IFormFile file) => File = file;
    }
}
