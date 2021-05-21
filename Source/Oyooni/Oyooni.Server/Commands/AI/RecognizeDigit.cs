using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI.DigitRecognition;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents the recognize digit command containing the request and the handler types
    /// </summary>
    public class RecognizeDigit
    {
        /// <summary>
        /// Represents the request for the <see cref="RecognizeDigit"/> command
        /// </summary>
        public class Request : IRequest<int>
        {
            /// <summary>
            /// The image file containing the digit to be recognized
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
        public class Handler : IRequestHandler<Request, int>
        {
            /// <summary>
            /// The digit recognizer service
            /// </summary>
            protected readonly IDigitRecognizer _digitRecognizer;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IDigitRecognizer digitRecognizer)
            {
                _digitRecognizer = digitRecognizer;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<int> Handle(Request request, CancellationToken token = default)
            {
                // Container for image data in base64 format
                var imageData = string.Empty;

                // Create a memory stream to host the data
                using (var memoryStream = new MemoryStream())
                {
                    // Copy the image data to the memory stream
                    await request.ImageFile.CopyToAsync(memoryStream, token);

                    // Save the image data to a temp file
                    imageData = Convert.ToBase64String(memoryStream.ToArray());
                }

                // Recognize result from image data
                var result1 = await _digitRecognizer.RecognizeDigitFromImageData(imageData, token);

                return result1;

                #region By Image

                //// Get image extension
                //var imageExtension = Path.GetExtension(request.ImageFile.FileName);

                //// Create a temp file and replace the default .tmp extension with the image file's extension
                //var tempFilePath = Path.GetTempFileName();

                //// Delete the temp file as it is 
                //File.Delete(tempFilePath);

                //// Change the extension of the temp file
                //var imageTempFile = Path.ChangeExtension(tempFilePath, imageExtension);

                //using (var stream = new FileStream(imageTempFile, FileMode.Create, FileAccess.Write))
                //{
                //    // Copy the image data to the memory stream
                //    await request.ImageFile.CopyToAsync(stream, token);
                //}

                //// Recognize digit from image file
                //var result2 = await _digitRecognizer.RecognizeDigitFromImage(imageTempFile, token);

                //// Delete the temp image file if it exists
                //if (File.Exists(imageTempFile)) File.Delete(imageTempFile);

                //// Return the result
                //return result2;

                #endregion
            }
        }
    }
}
