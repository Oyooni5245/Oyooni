using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI.TextRecogntion;
using Oyooni.Server.Services.General;
using System.IO;
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
        public class Request : IRequest<(string, string[])>
        {
            /// <summary>
            /// The file of the image to be used for recognizing the text
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// Indicates whether the pased image is a docuement image or just a general image
            /// </summary>
            public bool IsFromDocument { get; set; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(IFormFile imageFile, bool isFromDocument)
                => (ImageFile, IsFromDocument) = (imageFile, isFromDocument);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, (string, string[])>
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
            public async Task<(string, string[])> Handle(Request request, CancellationToken token)
            {
                // Get a temp file of the image file passed
                var imageTempFile = await _imageService.GetTempFileOfImage(request.ImageFile, token);

                // Recognize the text from the image file
                var result = await _textRecognitionService.RecognizeTextAsync(imageTempFile, request.IsFromDocument, token);

                // Delete the temp image
                File.Delete(imageTempFile);

                // Return results
                return result;
            }
        }
    }
}
