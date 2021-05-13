using MediatR;
using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Constants;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.Accounts.TokenProviders;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.Accounts
{
    /// <summary>
    /// Represents a login command containing the request and the handler types
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Represents the request type for the login command
        /// </summary>
        public class Request : IRequest<IAuthToken>
        {
            /// <summary>
            /// The user's email
            /// </summary>
            public string Email { get; }

            /// <summary>
            /// The user's password
            /// </summary>
            public string Password { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class
            /// </summary>
            public Request(string email, string password) => (Email, Password) = (email, password);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, IAuthToken>
        {
            /// <summary>
            /// Manger used to do operations on the <see cref="AppUser"/> class
            /// </summary>
            protected readonly UserManager<AppUser> _userManager;

            /// <summary>
            /// The authorization token provider used to provide auth tokens
            /// </summary>
            protected readonly IAuthorizationTokenProvider _tokenProvider;

            /// <summary>
            /// Contructs a new intance of the <see cref="Handler"/> class
            /// </summary>
            public Handler(UserManager<AppUser> userManager,
                IAuthorizationTokenProvider tokenProvider)
            {
                _userManager = userManager;
                _tokenProvider = tokenProvider;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<IAuthToken> Handle(Request request, CancellationToken token = default)
            {
                // Get the user by username (since the username in our case is the same as the email)
                var user = await _userManager.FindByNameAsync(request.Email);

                // if the user is not found
                if (user is null)
                    throw new NotFoundException(Responses.Accounts.InvalidLogin);

                // If the password is incorrect
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    throw new BadRequestException(Responses.Accounts.InvalidLogin);

                // If the user has not confirmed his email
                //if (!user.EmailConfirmed)
                //    throw new BadRequestException(Responses.Account.EmailNotVerified);

                // Generate and return an auth token
                return await _tokenProvider.GenerateAuthTokenForUserAsync(user);
            }
        }
    }
}
