using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request to recognize the bank note in an image file
    /// </summary>
    public class DetectBankNotetRequest
    {
        /// <summary>
        /// The image file to recognize the digit in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DetectBankNotetRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="DetectBankNotetRequest"/> class using the passed parameters
        /// </summary>
        public DetectBankNotetRequest(IFormFile file) => File = file;
    }
}
