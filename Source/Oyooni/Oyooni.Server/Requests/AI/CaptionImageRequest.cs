using Microsoft.AspNetCore.Http;
using Oyooni.Server.Constants;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request for captioning an image
    /// </summary>
    public class CaptionImageRequest
    {
        /// <summary>
        /// The image file to do the captioning for
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CaptionImageRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="CaptionImageRequest"/> class using the passed parameters
        /// </summary>
        public CaptionImageRequest(IFormFile file) => File = file;
    }
}
