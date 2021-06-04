using MediatR;
using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Constants;
using Oyooni.Server.Data;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Extensions;
using Oyooni.Server.Services.Accounts.TokenProviders;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.Accounts
{
    /// <summary>
    /// Represents a sign up command containing the request and the handler types
    /// </summary>
    public class Signup
    {
        /// <summary>
        /// Represents the request type for the sign up command
        /// </summary>
        public class Request : IRequest<IAuthToken>
        {
            /// <summary>
            /// The user's first name
            /// </summary>
            public string FirstName { get; }

            /// <summary>
            /// The user's last name
            /// </summary>
            public string LastName { get; }

            /// <summary>
            /// The user's email address
            /// </summary>
            public string Email { get; }

            /// <summary>
            /// The user's password
            /// </summary>
            public string Password { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class
            /// </summary>
            public Request(string firstName, string lastName, string email, string passwword)
                => (FirstName, LastName, Email, Password) = (firstName, lastName, email, passwword);
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
            /// Application context used to do database operations
            /// </summary>
            protected readonly IApplicationDbContext _context;

            /// <summary>
            /// The authorization token provider used to provide auth tokens
            /// </summary>
            protected readonly IAuthorizationTokenProvider _tokenProvider;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class
            /// </summary>
            public Handler(UserManager<AppUser> userManager,
                IApplicationDbContext context,
                IAuthorizationTokenProvider tokenProvider)
            {
                _userManager = userManager;
                _context = context;
                _tokenProvider = tokenProvider;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<IAuthToken> Handle(Request request, CancellationToken token)
            {
                // Get the user by email
                var userByEmail = await _userManager.FindByEmailAsync(request.Email);

                // If the user is not found
                if (userByEmail is not null)
                    throw new AlreadyExistException(message: Responses.Accounts.AlreadyExist);

                // Instantiate a new AppUser and trim it
                var newUser = new AppUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.Email
                }.Trim();

                // Add the new user
                await _userManager.CreateAsync(newUser, request.Password);

                // Return new IAuthToken
                return await _tokenProvider.GenerateAuthTokenForUserAsync(newUser, token);
            }
        }
    }
}
