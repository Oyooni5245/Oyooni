using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Common;
using Oyooni.Server.Constants;
using Oyooni.Server.Dtos.AI;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AI;
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
        [Route(ApiRoutes.AI.DetectBankNote)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BankNoteDetectionDto))]
        public async Task<IActionResult> DetectBankNote([FromForm]DetectBankNotetRequest request, CancellationToken token = default)
        {
            // Send a command to recognize and get the recognized digit
            var result = await _mediator.Send(request.ToMediatorRequest(), token);
            
            // Return the recognition result
            return ApiOk(Responses.AI.BankNoteDetectionSuccess, new BankNoteDetectionDto(result));
        }

        [HttpPost]
        [Route(ApiRoutes.AI.RecognizeColor)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ColorRecognitionDto))]
        public async Task<IActionResult> RecognizeColor([FromForm] RecognizeColorRequest request, CancellationToken token = default)
        {
            // Send a command to recognize and get the recognized Colors
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the recognition result
            return ApiOk(Responses.AI.ColorRecognitionSuccess, new ColorRecognitionDto(result));
        }

        [HttpPost]
        [Route(ApiRoutes.AI.CaptionImage)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ImageCaptioningDto))]
        public async Task<IActionResult> CaptionImage([FromForm] CaptionImageRequest request, CancellationToken token = default)
        {
            // Send a command to caption the image
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the result
            return ApiOk(Responses.AI.ImageCaptionedSuccess, new ImageCaptioningDto(result));
        }

        [HttpPost]
        [Route(ApiRoutes.AI.RecognizeText)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TextRecognitionDto))]
        public async Task<IActionResult> RecognizeText([FromForm] RecognizeTextRequest request, CancellationToken token = default)
        {
            // Send a command to caption the image
            (var brandName, var subText)= await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the result
            return ApiOk(Responses.AI.TextRecognitionSuccess, new TextRecognitionDto(subText, brandName));
        }
    }
}
