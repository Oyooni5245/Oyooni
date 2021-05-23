using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyooni.Server.Requests.AI
{
    /// <summary>
    /// Represents a request for visually answering a question on an image
    /// </summary>
    public class VQARequest
    {
        /// <summary>
        /// The question related to the image
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// The image file
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public VQARequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="VQARequest"/> class using the passed parameters
        /// </summary>
        public VQARequest(string question, IFormFile file) => (Question, File) = (question, file);
    }
}
