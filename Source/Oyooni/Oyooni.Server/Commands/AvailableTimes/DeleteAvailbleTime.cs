using MediatR;
using Oyooni.Server.Constants;
using Oyooni.Server.Data;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AvailableTimes
{
    /// <summary>
    /// Represents a delete availble time command containing the request and the handler types
    /// </summary>
    public class DeleteAvailbleTime
    {
        /// <summary>
        /// Represents a request for the <see cref="DeleteAvailbleTime"/> command
        /// </summary>
        public class Request : IRequest<Unit>
        {
            /// <summary>
            /// The available time identifier
            /// </summary>
            public string AvailableTimeId { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            public Request(string availableTimeId) => AvailableTimeId = availableTimeId;
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request>
        {
            /// <summary>
            /// The context that is used to do db operations
            /// </summary>
            protected readonly IApplicationDbContext _context;

            /// <summary>
            /// The logged in user service
            /// </summary>
            protected readonly ILoggedInUserService _loggedInUserService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(IApplicationDbContext context,
                ILoggedInUserService loggedInUserService)
            {
                _context = context;
                _loggedInUserService = loggedInUserService;
            }

            /// <summary>
            /// Handles when the <see cref="Request"/> is sent
            /// </summary>
            public async Task<Unit> Handle(Request request, CancellationToken token = default)
            {
                // Get the available time by id
                var time = await _context.AvailableTimes.FindAsync(new object[] { request.AvailableTimeId }, token);

                // If the time does not exist
                if (time is null)
                    throw new NotFoundException(message: Responses.AvailableTimes.TimeNotFound);

                // Check if the time belonges to the current user
                if (!time.BelongsTo(_loggedInUserService.UserId))
                    throw new UnAuthorizedException();

                // Remove the time from the db
                _context.AvailableTimes.Remove(time);

                // Save all changes to the db
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
