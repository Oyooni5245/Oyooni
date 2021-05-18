using MediatR;
using Microsoft.EntityFrameworkCore;
using Oyooni.Server.Data;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Services.Accounts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Queries.AvailableTimes
{
    /// <summary>
    /// Represents a get available times command containing the request and handler types
    /// </summary>
    public class GetAvailableTimes
    {
        /// <summary>
        /// Represents the request for the <see cref="GetAvailableTimes"/> command
        /// </summary>
        public class Request : IRequest<IEnumerable<AvailableTime>> { }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/>
        /// </summary>
        public class Handler : IRequestHandler<Request, IEnumerable<AvailableTime>>
        {
            /// <summary>
            /// The db context used to do db operations
            /// </summary>
            protected readonly IApplicationDbContext _context;

            /// <summary>
            /// The logged in user service
            /// </summary>
            protected readonly ILoggedInUserService _loggedInUserService;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class using the passed parameters
            /// </summary>
            public Handler(ILoggedInUserService loggedInUserService,
                IApplicationDbContext context)
            {
                _loggedInUserService = loggedInUserService;
                _context = context;
            }

            /// <summary>
            /// Handles when the <see cref="Request"/> is sent
            /// </summary>
            public async Task<IEnumerable<AvailableTime>> Handle(Request request, CancellationToken cancellationToken)
            {
                // Return all available times for the current user
                return await _context.AvailableTimes.AsNoTracking()
                    .Where(a => a.UserId == _loggedInUserService.UserId).ToListAsync();
            }
        }

    }
}
