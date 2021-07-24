using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using Oyooni.Server.Services.AI.ColorRecognition;
using System.Collections.Generic;
using Oyooni.Server.Services.General;
using Oyooni.Server.Enumerations;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Constants;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents the recognize color command containing the request and the handler types
    /// </summary>
    public class RecognizeColors
    {
        /// <summary>
        /// Represents the request for the <see cref="RecognizeColors"/> command
        /// </summary>
        public class Request : IRequest<RecognizedColor>
        {
            /// <summary>
            /// The image file containing the colors to be recognized
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> using the passed parameters
            /// </summary>
            public Request(IFormFile formFile) => (ImageFile) = (formFile);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, RecognizedColor>
        {
            /// <summary>
            /// The color recognizer service
            /// </summary>
            protected readonly IColorRecognitionService _colorRecognizer;

            /// <summary>
            /// The image service
            /// </summary>
            protected readonly IImageService _imageService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IColorRecognitionService colorRecognizer,
                IImageService imageService)
            {
                _colorRecognizer = colorRecognizer;
                _imageService = imageService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<RecognizedColor> Handle(Request request, CancellationToken token = default)
            {
                // Get a temp file of the image file passed
                using (var imageTempFile = await _imageService.GetTempFileOfImage(request.ImageFile, token))
                {
                    try
                    {
                        // Recognize color from image file
                        var result = await _colorRecognizer.RecognizeColorInImageAsync(imageTempFile.PathToTempFile, token);

                        // Return results
                        return result;
                    }
                    catch (TimeoutException) { throw new ServiceUnavailableException(Responses.General.ServiceUnavailable); }
                    catch (Exception) { throw new Exception(); }
                    finally { imageTempFile.Dispose(); }
                }
            }
        }
    }
}
