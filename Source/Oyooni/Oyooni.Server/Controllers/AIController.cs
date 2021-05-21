using MediatR;
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
    }
}
