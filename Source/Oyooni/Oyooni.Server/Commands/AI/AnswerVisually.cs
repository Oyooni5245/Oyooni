using MediatR;
using Microsoft.AspNetCore.Http;
using Oyooni.Server.Services.AI.VAQ;
using Oyooni.Server.Services.General;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AI
{
    /// <summary>
    /// Represents a command for visually answering a question on an image, contains the request and the handler types
    /// </summary>
    public class AnswerVisually
    {
        /// <summary>
        /// Represents the request for the <see cref="AnswerVisually"/> command
        /// </summary>
        public class Request : IRequest<string>
        {
            /// <summary>
            /// The question to be answered
            /// </summary>
            public string Question { get; }

            /// <summary>
            /// The file of the image to be used for answering the question
            /// </summary>
            public IFormFile ImageFile { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(string question, IFormFile imageFile) => (Question, ImageFile) = (question, imageFile);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, string>
        {
            /// <summary>
            /// The visual question answering service
            /// </summary>
            protected readonly IVisualQuestionAnsweringService _vaqService;

            /// <summary>
            /// The image service
            /// </summary>
            protected readonly IImageService _imageService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IVisualQuestionAnsweringService vaqService,
                IImageService imageService)
            {
                _vaqService = vaqService;
                _imageService = imageService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<string> Handle(Request request, CancellationToken token)
            {
                return await _vaqService.AnswerAsync(request.Question,
                    await _imageService.GetBase64ImageDataAsync(request.ImageFile, token), token);
            }
        }
    }
}
