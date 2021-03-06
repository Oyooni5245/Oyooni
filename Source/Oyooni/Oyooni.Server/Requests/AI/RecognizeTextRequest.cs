using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request for recognizing a text in an image
    /// </summary>
    public class RecognizeTextRequest
    {
        /// <summary>
        /// The image file to recognize the text in
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Indicates whether the passed image is a document image or just a general image
        /// </summary>
        public bool IsFromDocument { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeTextRequest() { }

        /// <summary>
        /// Constructs a new instanc of the <see cref="DetectBankNotetRequest"/> class using the pased parameters
        /// </summary>
        public RecognizeTextRequest(IFormFile file, bool isFromDocument) => (File, IsFromDocument) = (file, isFromDocument);
    }
}
