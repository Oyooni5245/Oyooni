﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using Oyooni.Server.Services.AI.ColorRecognition;
using System.Collections.Generic;

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
        public class Request : IRequest<Dictionary<string, float>>
        {
            /// <summary>
            /// The image file containing the colors to be recognized
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// The number of colors to be detected in the image
            /// </summary>
            public int NumberOfColorsToDetect { get; set; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> using the passed parameters
            /// </summary>
            public Request(IFormFile formFile, int numberOfColorsToDetect = 3)
                => (ImageFile, NumberOfColorsToDetect) = (formFile, numberOfColorsToDetect);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, Dictionary<string, float>>
        {
            /// <summary>
            /// The color recognizer service
            /// </summary>
            protected readonly IColorRecognizer _colorRecognizer;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IColorRecognizer colorRecognizer)
            {
                _colorRecognizer = colorRecognizer;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<Dictionary<string, float>> Handle(Request request, CancellationToken token = default)
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
                var result1 = await _colorRecognizer.RecognizeColorsInImageDataAsync(imageData, request.NumberOfColorsToDetect, token);

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

                //// Recognize color from image file
                //var result2 = await _colorRecognizer.RecognizeColorFromImage(imageTempFile, token);

                //// Delete the temp image file if it exists
                //if (File.Exists(imageTempFile)) File.Delete(imageTempFile);

                //// Return the result
                //return result2;

                #endregion
            }
        }
    }
}