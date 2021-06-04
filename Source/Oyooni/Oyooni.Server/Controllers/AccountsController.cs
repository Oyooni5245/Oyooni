using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Common;
using Oyooni.Server.Constants;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Extensions;
using Oyooni.Server.Queries.Accounts;
using Oyooni.Server.Requests.Accounts;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Controllers
{
    /// <summary>
    /// Represents the accounts controller class for handling accounts api endpoints
    /// </summary>
    [Authorize]
    public class AccountsController : BaseApiController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> service</param>
        public AccountsController(IMediator mediator, IStringLocalizer<Program> localizer) : base(mediator, localizer) { }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Accounts.Login)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IAuthToken>))]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken token = default)
        {
            // Send a login command
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return an ok result with the resulting data
            return ApiOk(message: Responses.Accounts.LoginSuccess, data: result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Accounts.Signup)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IAuthToken>))]
        public async Task<IActionResult> Signup(SignupRequest request, CancellationToken token = default)
        {
            // Send a signup command
            var data = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return an ok result along with the token
            return ApiOk(data: data, message: Responses.Accounts.SignupSuccess);
        }

        [HttpPost]
        [Route(ApiRoutes.Accounts.RefreshToken)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IAuthToken>))]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken token = default)
        {
            // Send a refresh token command
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return an ok result with the resulting data
            return ApiOk(data: result);
        }

        [HttpGet]
        [Route(ApiRoutes.Accounts.Profile)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppUserDto>))]
        public async Task<IActionResult> GetProfile(CancellationToken token = default)
        {
            // Send a get profile query
            var result = await _mediator.Send(new GetProfile.Request(), token);

            // return the ok result with the resulting data
            return ApiOk(data: result.ToAppUserDto(), message: Responses.Accounts.ProfileRetrieved);
        }

        [HttpPut]
        [Route(ApiRoutes.Accounts.Profile)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<AppUserDto>))]
        public async Task<IActionResult> EditProfile(EditProfileRequest request, CancellationToken token = default)
        {
            // Send an Edit profile command
            var result = await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the resulting data
            return ApiOk(data: result.ToAppUserDto(), message: Responses.Accounts.ProfileUpdateSuccess);
        }

        [HttpPut]
        [Route(ApiRoutes.Accounts.ChangePassword)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken token = default)
        {
            // Send an Edit profile command
            await _mediator.Send(request.ToMediatorRequest(), token);

            // Return the resulting data
            return ApiOk(message: Responses.Accounts.ProfileUpdateSuccess);
        }
    }
}
