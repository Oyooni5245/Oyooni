using MediatR;
using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Constants;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.Accounts
{
    /// <summary>
    /// Represents a change password command containing the request and the handler types
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// Represents the request for the <see cref="ChangePassword"/> command
        /// </summary>
        public class Request : IRequest<Unit>
        {
            /// <summary>
            /// The user's old password
            /// </summary>
            public string OldPassword { get; }

            /// <summary>
            /// The user's new password
            /// </summary>
            public string NewPassword { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(string oldPassword, string newPassword)
                => (OldPassword, NewPassword) = (oldPassword, newPassword);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, Unit>
        {
            /// <summary>
            /// The logged in user service
            /// </summary>
            protected readonly ILoggedInUserService _loggedInUserService;
            protected readonly UserManager<AppUser> _userManager;

            /// <summary>
            /// Constructs an instance of the <see cref="Handler"/> class using the passed parameters
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
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken = default)
            {
                // Get the user by id
                var user = await _userManager.FindByIdAsync(_loggedInUserService.UserId);

                // Check if the old password is correct
                if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
                    throw new BadRequestException(message: Responses.Accounts.IncorrectPassword);

                // Change the password to the new password
                await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

                // Return nothing
                return Unit.Value;
            }
        }
    }
}
