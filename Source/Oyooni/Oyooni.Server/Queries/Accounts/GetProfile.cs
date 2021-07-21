using MediatR;
using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Queries.Accounts
{
    /// <summary>
    /// Represents a get profile command containing the requets and the handler types
    /// </summary>
    public class GetProfile
    {
        /// <summary>
        /// Represents the request type for the <see cref="GetProfile"/> class
        /// </summary>
        public class Request : IRequest<AppUser> { }

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
            /// The manager used for doing operations related to the <see cref="AppUser"/>
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
            public async Task<AppUser> Handle(Request request, CancellationToken token = default)
            {
                return await _userManager.FindByIdAsync(_loggedInUserService.UserId);
            }
        }
    }
}
