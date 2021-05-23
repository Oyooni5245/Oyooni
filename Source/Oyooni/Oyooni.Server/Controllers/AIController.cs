﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Common;
using Oyooni.Server.Constants;
using Oyooni.Server.Dtos.AI;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AI;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Controllers
{
    /// <summary>
    /// Represents the controller for handling requests related to the AI services provided by the application
    /// </summary>
    public class AIController : BaseApiController
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AIController"/> class using the passed parameters
        /// </summary>
        public AIController(IMediator mediator, IStringLocalizer<Program> localizer) : base(mediator, localizer) { }

        [HttpPost]
        [Route(ApiRoutes.AI.RecognizeDigit)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<DigitRecognitionResultDto>))]
        public async Task<IActionResult> RecognizeDigit([FromForm]RecognizeDigitRequest request, CancellationToken token = default)
        {
            // Send a command to recognize and get the recognized digit
            var result = await _mediator.Send(request.ToMediatorRequest(), token);
            
            // Return the recognition result
            return ApiOk(data: new DigitRecognitionResultDto(result), message: Responses.AI.DigitRecognitionSuccess);
        }

        [HttpPost]
        [Route(ApiRoutes.AI.RecognizeColor)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<DigitRecognitionResultDto>))]
        public async Task<IActionResult> RecognizeColor([FromForm] RecognizeColorRequest request, CancellationToken token = default)
        {
            // Send a command to recognize and get the recognized Colors
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the recognition result
            return ApiOk(data: new ColorRecognitionResultDto(result), message: Responses.AI.ColorRecognitionSuccess);
        }

        [HttpPost]
        [Route(ApiRoutes.AI.CaptionImage)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<ImageCaptioningResultDto>))]
        public async Task<IActionResult> CaptionImage([FromForm] CaptionImageRequest request, CancellationToken token = default)
        {
            // Send a command to caption the image
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the result
            return ApiOk(data: new ImageCaptioningResultDto(result), message: Responses.AI.ImageCaptionedSuccess);
        }

        [HttpPost]
        [Route(ApiRoutes.AI.EnglishVQA)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<VQAResultDto>))]
        public async Task<IActionResult> VisuallyAnswer([FromForm] VQARequest request, CancellationToken token = default)
        {
            // Send a command to visually answer a question
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the result
            return ApiOk(data: new VQAResultDto(result), message: Responses.AI.ColorRecognitionSuccess);
        }
    }
}
