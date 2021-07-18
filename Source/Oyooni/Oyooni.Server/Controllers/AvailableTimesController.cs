using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Commands.AvailableTimes;
using Oyooni.Server.Common;
using Oyooni.Server.Constants;
using Oyooni.Server.Dtos.AvailableTimes;
using Oyooni.Server.Queries.AvailableTimes;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AvailableTimes;

namespace Oyooni.Server.Controllers
{
    /// <summary>
    /// Represents the available times controller class for handling api endpoints related to the available times
    /// </summary>
    [Authorize]
    public class AvailableTimesController : BaseApiController
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AvailableTimesController"/> class using the passed parameters
        /// </summary>
        public AvailableTimesController(IMediator mediator, IStringLocalizer<Program> localizer)
            : base(mediator, localizer) { }

        [HttpGet]
        [Route(ApiRoutes.AvailableTimes.Base)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IDictionary<int, IEnumerable<AvailableTimeDto>>>))]
        public async Task<IActionResult> Get(CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetAvailableTimes.Request(), token);

            return ApiOk(data: result.ToGroupedAvailableTimeDto(), message: Responses.AvailableTimes.AvailableTimesRetrieved);
        }

        [HttpPost]
        [Route(ApiRoutes.AvailableTimes.Base)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<AvailableTimeDto>>))]
        public async Task<IActionResult> Post(AddAvailableTimeRequest request, CancellationToken token = default)
        {
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            return ApiOk(data: result.ToAvailableTimeDto(), message: Responses.AvailableTimes.TimeAdded);
        }

        [HttpDelete]
        [Route(ApiRoutes.AvailableTimes.DeleteAvailbleTime)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(string id, CancellationToken token = default)
        {
            await _mediator.Send(new DeleteAvailbleTime.Request(id), token);

            return ApiOk(message: Responses.AvailableTimes.TimeDeleted);
        }
    }
}
