using MediatR;
using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Services.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.Accounts
{
    /// <summary>
    /// Represents an edit profile command containing the requst and handler types
    /// </summary>
    public class EditProfile
    {
        /// <summary>
        /// Represents the request for the edit <see cref="EditProfile"/> command
        /// </summary>
        public class Request : IRequest<AppUser>
        {
            /// <summary>
            /// The new first name of the user
            /// </summary>
            public string FirstName { get; }

            /// <summary>
            /// The new last name of the user
            /// </summary>
            public string LastName { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);

            /// <summary>
            /// Returns a new timmed instance of the <see cref="Request"/> class
            /// </summary>
            /// <returns></returns>
            public Request AsTrimmed() => new Request(FirstName.Trim(), LastName.Trim());
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, AppUser>
        {
            /// <summary>
            /// The logged in user service
            /// </summary>
            protected readonly ILoggedInUserService _loggedInUserService;

            /// <summary>
            /// The user manager used to do operations related to the user
            /// </summary>
            protected readonly UserManager<AppUser> _userManager;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(ILoggedInUserService loggedInUserService,
                UserManager<AppUser> userManager)
            {
                _loggedInUserService = loggedInUserService;
                _userManager = userManager;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<AppUser> Handle(Request request, CancellationToken cancellationToken)
            {
                // Get the current user
                var user = await _userManager.FindByIdAsync(_loggedInUserService.UserId);

                // Trim the request object
                request = request.AsTrimmed();

                // Set the new values
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                // Update the user
                await _userManager.UpdateAsync(user);

                // Return the user
                return user;
            }
        }
    }
}
