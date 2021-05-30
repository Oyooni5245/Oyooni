using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request to recognize the bank note in an image file
    /// </summary>
    public class RecognizeBankNotetRequest
    {
        /// <summary>
        /// The image file to recognize the digit in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeBankNotetRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RecognizeBankNotetRequest"/> class using the passed parameters
        /// </summary>
        public RecognizeBankNotetRequest(IFormFile file) => File = file;
    }
}
