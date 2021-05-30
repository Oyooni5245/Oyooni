using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI.TextRecogntion;
using Oyooni.Server.Services.General;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents a command for recogniting text in an image, contains the request and the handler types
    /// </summary>
    public class RecognizeText
    {
        /// <summary>
        /// Represents the request for the <see cref="RecognizeText"/> command
        /// </summary>
        public class Request : IRequest<string>
        {
            /// <summary>
            /// The file of the image to be used for recognizing the text
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
            /// The visual question answering service
            /// </summary>
            protected readonly ITextRecognitionService _textRecognitionService;

            /// <summary>
            /// The image service
            /// </summary>
            protected readonly IImageService _imageService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(ITextRecognitionService textRecognitionService,
                IImageService imageService)
            {
                _textRecognitionService = textRecognitionService;
                _imageService = imageService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<string> Handle(Request request, CancellationToken token)
            {
                return await _textRecognitionService.RecognizeTextAsync(
                    await _imageService.GetBase64ImageDataAsync(request.ImageFile, token), token);
            }
        }
    }
}
