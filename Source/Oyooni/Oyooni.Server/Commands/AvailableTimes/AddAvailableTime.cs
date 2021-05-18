using MediatR;
using Microsoft.EntityFrameworkCore;
using Oyooni.Server.Constants;
using Oyooni.Server.Data;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.AvailableTimes
{
    /// <summary>
    /// Represents an add available time command containing request and handler types
    /// </summary>
    public class AddAvailableTime
    {
        /// <summary>
        /// Represents the request type for the <see cref="AddAvailableTime"/> command
        /// </summary>
        public class Request : IRequest<IEnumerable<AvailableTime>>
        {
            /// <summary>
            /// The day of the week
            /// </summary>
            public DayOfWeek DayOfWeek { get; }

            /// <summary>
            /// The start time
            /// </summary>
            public TimeSpan From { get; }

            /// <summary>
            /// The end time
            /// </summary>
            public TimeSpan To { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class using the passed parameters
            /// </summary>
            /// <param name="dayOfWeek"></param>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public Request(DayOfWeek dayOfWeek, TimeSpan from, TimeSpan to) => (DayOfWeek, From, To) = (dayOfWeek, from, to);
        }

        /// <summary>
        /// Represents the handler type for the <see cref="Request"/>
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

            public Handler(IApplicationDbContext context,
                ILoggedInUserService loggedInUserService)
            {
                _context = context;
                _loggedInUserService = loggedInUserService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            public async Task<IEnumerable<AvailableTime>> Handle(Request request, CancellationToken token = default)
            {
                var start = request.From;
                var end = request.To;
                var dayOfWeek = request.DayOfWeek;

                // Get all the available times of the user according to the passed day of the week
                // and order by the From property
                var currentAvailableTimes = await _context.AvailableTimes
                    .Where(a => a.UserId == _loggedInUserService.UserId && a.DayOfWeekId == (int)dayOfWeek)
                    .OrderBy(a => a.From).ToListAsync(token);

                // If the time to be added already exists
                if (currentAvailableTimes.Any(a => a.From == start && a.To == end))
                    throw new BadRequestException(message: Responses.AvailableTimes.SameTimeExists);

                // If the time to be added is contained within another time
                if (currentAvailableTimes.Any(a => a.From <= start && a.To >= end))
                    throw new BadRequestException(message: Responses.AvailableTimes.TimeAlreadyCovered);

                // Get the contained time for the start and the end time
                var containingTimeForStart = currentAvailableTimes.FirstOrDefault(a => a.From <= start && a.To >= start);
                var containingTimeForEnd = currentAvailableTimes.FirstOrDefault(a => a.From <= end && a.To >= end);

                // Get all times that are between the range of the new time
                var timesBetweenNewTime = currentAvailableTimes.Where(a => a.From >= start && a.To <= end).ToList();

                // Check if there is an interference of any kind
                // Either From is contained or To is contained or new time contained existing times
                var interferenceExists = containingTimeForStart != null || containingTimeForEnd != null || timesBetweenNewTime.Any();

                // If new time has its own range
                if (!interferenceExists)
                {
                    // Create the new time
                    var newTime = new AvailableTime
                    {
                        DayOfWeek = request.DayOfWeek,
                        From = request.From,
                        To = request.To
                    };

                    // Add it to the db and save all changes
                    await _context.AvailableTimes.AddAsync(newTime, token);
                    await _context.SaveChangesAsync(_loggedInUserService.UserId, token);

                    // Add the new time to the local times collection
                    currentAvailableTimes.Add(newTime);

                    // Return the times
                    return currentAvailableTimes;
                }
                
                // If start and end times are not contained in any previous time
                if (containingTimeForStart is null && containingTimeForEnd is null)
                {
                    // Create the new time
                    var newTime = new AvailableTime
                    {
                        DayOfWeek = request.DayOfWeek,
                        From = request.From,
                        To = request.To
                    };

                    // Add it to the db
                    await _context.AvailableTimes.AddAsync(newTime, token);

                    // Add the new time to the local times collection
                    currentAvailableTimes.Add(newTime);

                    // Delete the times in between locally and from the db
                    _context.AvailableTimes.RemoveRange(timesBetweenNewTime);

                    // Remove times that are between the range of the newly created time
                    foreach (var timeBetween in timesBetweenNewTime)
                        currentAvailableTimes.Remove(timeBetween);

                    // Save changes to the db
                    await _context.SaveChangesAsync(_loggedInUserService.UserId, token);

                    // Return the times
                    return currentAvailableTimes;
                }

                // If start time is not contained and the end time is contained
                if (containingTimeForStart is null && containingTimeForEnd is not null)
                {
                    // Set the start of the containing end time to the start of the new time to be add
                    containingTimeForEnd.From = start;

                    // Remove the containing time from the times between if it is there
                    timesBetweenNewTime.RemoveAll(a => a.Id == containingTimeForEnd.Id);

                    // Remove the between times from the db
                    _context.AvailableTimes.RemoveRange(timesBetweenNewTime);

                    // Remove the between times from the local list
                    foreach (var timeBetween in timesBetweenNewTime)
                        currentAvailableTimes.Remove(timeBetween);

                    // Save changes
                    await _context.SaveChangesAsync(_loggedInUserService.UserId, token);

                    // Return the available times
                    return currentAvailableTimes;
                }

                // If start time is contained and the end time is not contained
                if (containingTimeForStart is not null && containingTimeForEnd is null)
                {
                    // Make the from of the containing start time stretch to the end of the time to be added
                    containingTimeForStart.To = end;

                    // Remove the containing time from the between times if it is there
                    timesBetweenNewTime.RemoveAll(a => a.Id == containingTimeForStart.Id);

                    // Remove the between times from the db
                    _context.AvailableTimes.RemoveRange(timesBetweenNewTime);

                    // Remove the between times from the local list
                    foreach (var betweenTime in timesBetweenNewTime)
                        currentAvailableTimes.Remove(betweenTime);

                    // Save all changes to the db
                    await _context.SaveChangesAsync(_loggedInUserService.UserId, token);

                    // Return the user's current available times
                    return currentAvailableTimes;
                }

                // If the start time and the end time are contained
                if (containingTimeForStart is not null && containingTimeForEnd is not null)
                {
                    // Make the containing start stretch to the containing end
                    containingTimeForStart.To = containingTimeForEnd.To;

                    // Remove the containing start time from the between times if it is there
                    timesBetweenNewTime.RemoveAll(a => a.Id == containingTimeForStart.Id);

                    // Add the containing end time to the between times to be removed later
                    if (!timesBetweenNewTime.Contains(containingTimeForEnd))
                        timesBetweenNewTime.Add(containingTimeForEnd);

                    // Remove all between times from db
                    _context.AvailableTimes.RemoveRange(timesBetweenNewTime);

                    // Remove all between times from the local list
                    foreach (var betweenTime in timesBetweenNewTime)
                        currentAvailableTimes.Remove(betweenTime);

                    // Save Changes to db
                    await _context.SaveChangesAsync(_loggedInUserService.UserId, token);

                    // Return the user's current available times
                    return currentAvailableTimes;
                }

                throw new BaseException(message: Responses.General.ErrorOccured, HttpStatusCode.InternalServerError);
            }
        }
    }
}
