using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Common;
using System.Net;

namespace Oyooni.Server.Controllers
{
    /// <summary>
    /// Represents the base api controller class
    /// </summary>
    [ApiController]
    [Route("/api")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IStringLocalizer<Program> _localizer;
        
        /// <summary>
        /// Constructs a new object of type <see cref="BaseApiController"/>
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> service</param>
        protected BaseApiController(IMediator mediator, IStringLocalizer<Program> localizer)
            => (_mediator, _localizer) = (mediator, localizer);

        /// <summary>
        /// Returns an <see cref="HttpStatusCode.OK"/> result with an object result of type <see cref="ApiResponse"/>
        /// along with the <see cref="TData"/> as data argument
        /// </summary>
        /// <typeparam name="TData">The type of data to be returned</typeparam>
        /// <param name="message">The message to be returned</param>
        /// <param name="data">The data to be returned</param>
        protected IActionResult ApiOk<TData>(string message = null, TData data = default)
            where TData : class => Ok(new ApiResponse<TData>(message, data));

        /// <summary>
        /// Returns an <see cref="HttpStatusCode.OK"/> result with an object result of type <see cref="ApiResponse"/>
        /// containing the response message
        /// </summary>
        /// <param name="message">The message to be returned</param>
        protected IActionResult ApiOk(string message = null) => Ok(new ApiResponse(message));
    }
}
