using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Constants;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.AI.ImageCaptioning;
using Oyooni.Server.Services.General;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents a command for captioning an image containing the request and the handler types
    /// </summary>
    public class CaptionImage
    {
        /// <summary>
        /// Represents the request for the <see cref="CaptionImage"/> command
        /// </summary>
        public class Request : IRequest<string>
        {
            /// <summary>
            /// The file of the image to be captioned
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(IFormFile imageFile) => ImageFile = imageFile;
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, string>
        {
            /// <summary>
            /// The image captioning service
            /// </summary>
            protected readonly IImageCaptioningService _imageCaptioningService;

            /// <summary>
            /// The image service
            /// </summary>
            protected readonly IImageService _imageService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IImageCaptioningService imageCaptioningService,
                IImageService imageService)
            {
                _imageCaptioningService = imageCaptioningService;
                _imageService = imageService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<string> Handle(Request request, CancellationToken token)
            {
                // Get a temp file of the image file passed
                using (var imageTempFile = await _imageService.GetTempFileOfImage(request.ImageFile, token))
                {
                    try
                    {
                        // Caption the temp image file
                        var caption = await _imageCaptioningService.CaptionImageAsync(imageTempFile.PathToTempFile, token);

                        // Return results
                        return caption;
                    } 
                    catch (Exception e) 
                    { throw; } finally { imageTempFile.Dispose(); }
                }
            }
        }
    }
}
