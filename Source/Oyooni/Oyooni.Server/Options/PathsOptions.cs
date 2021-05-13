using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Oyooni.Server.Options
{
    /// <summary>
    /// Represents the paths options
    /// </summary>
    public class PathsOptions
    {
        /// <summary>
        /// Constructors a new instance of the <see cref="PathsOptions"/> class using the passed parameters
        /// </summary>
        public PathsOptions(IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
        {
            // Set the web root path
            WebRootPath = env.WebRootPath;

            // Get the http request
            var request = contextAccessor.HttpContext.Request;

            // Set the main url (eg: https://www.google.com)
            MainUrl = $"{request.Scheme}://{request.Host}";
        }

        /// <summary>
        /// The web root path
        /// </summary>
        public string WebRootPath { get; set; }

        /// <summary>
        /// The main url
        /// </summary>
        public string MainUrl { get; }
    }
}
