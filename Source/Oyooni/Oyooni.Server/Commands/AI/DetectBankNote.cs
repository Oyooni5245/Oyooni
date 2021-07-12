using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Enumerations;
using Oyooni.Server.Services.AI.BankNoteDetection;
using Oyooni.Server.Services.General;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents the recognize bank note command containing the request and the handler types
    /// </summary>
    public class DetectBankNote
    {
        /// <summary>
        /// Represents the request for the <see cref="DetectBankNote"/> command
        /// </summary>
        public class Request : IRequest<SyrianBankNoteTypes>
        {
            /// <summary>
            /// The image file containing the bank note to be recognized
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> using the passed parameters
            /// </summary>
            public Request(IFormFile formFile) => ImageFile = formFile;
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, SyrianBankNoteTypes>
        {
            /// <summary>
            /// The digit detector service
            /// </summary>
            protected readonly ISyrianBankNoteDetectionService _bankNoteDetectionService;

            /// <summary>
            /// The image service
            /// </summary>
            protected readonly IImageService _imageService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(ISyrianBankNoteDetectionService digitRecognizer,
                IImageService imageService)
            {
                _bankNoteDetectionService = digitRecognizer;
                _imageService = imageService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<SyrianBankNoteTypes> Handle(Request request, CancellationToken token = default)
            {
                // Get a temp file of the image file passed
                var imageTempFile = await _imageService.GetTempFileOfImage(request.ImageFile, token);

                // Recognize digit from image file
                var result = await _bankNoteDetectionService.DetectBankNoteAsync(imageTempFile, token);

                // Delete the temp image
                File.Delete(imageTempFile);

                // Return results
                return result;
            }
        }
    }
}
