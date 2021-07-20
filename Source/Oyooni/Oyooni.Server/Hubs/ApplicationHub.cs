using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Oyooni.Server.Constants;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Extensions;
using Oyooni.Server.Services.Accounts;
using Oyooni.Server.Services.Cache;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Oyooni.Server.Hubs
{
    /// <summary>
    /// Represents the application's main hub
    /// </summary>
    public class ApplicationHub : Hub<IServerEvents>
    {
        /// <summary>
        /// The cache service used for the current hub
        /// </summary>
        protected readonly IHubCacheService _hubCacheService;
        protected readonly UserManager<AppUser> _userManager;
        protected readonly ILoggedInUserService _loggedInUserSerivce;
        protected readonly IStringLocalizer<Program> _stringLocalizer;

        public ApplicationHub(IHubCacheService hubCacheStorage,
            ILoggedInUserService loggedInUserSerivce,
            IStringLocalizer<Program> stringLocalizer,
            UserManager<AppUser> userManager)
        {
            _hubCacheService = hubCacheStorage;
            _loggedInUserSerivce = loggedInUserSerivce;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }

        /// <summary>
        /// Handles when a client connects to the hub
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            // If the user id is empty then it is a visually impaired person that is trying to connect to the signalR connection
            var isVolunteer = !_loggedInUserSerivce.UserId.IsNullOrEmptyOrWhiteSpaceSafe();

            // If it is not a volunteer
            if (!isVolunteer)
                // Add the connectionId to the cache as a viusally impaired connectionId
                await _hubCacheService.SetConnectionAsync(Context.ConnectionId, true);
            else
                // Add the connectionId to the cache as a volunteer connectionId
                await _hubCacheService.SetConnectionAsync(Context.ConnectionId, false, await _userManager.FindByIdAsync(_loggedInUserSerivce.UserId));

            //// Send the connection success to the caller
            await Clients.Caller.ConnectedSuccessfully(Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Handles when a client is disconnected from the hub
        /// </summary>
        /// <param name="exception">The exception thrown upon disconnection</param>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentConnectionId = Context.ConnectionId;
            var isVolunteer = !_loggedInUserSerivce.UserId.IsNullOrEmptyOrWhiteSpaceSafe();

            // If it was a volunteer
            if (isVolunteer)
            {
                // Check if the volunteer is in an existing call already
                // if yes then tell the visually impaired person that was getting the help that the volunteer disconnected
                (var volunteerInACall, var vIConnectionId) = await _hubCacheService.IsInACallAsync(currentConnectionId);

                if (volunteerInACall)
                {
                    await _hubCacheService.RemoveCallAsync(vIConnectionId);
                    await Clients.Client(vIConnectionId).VolunteerDisconnected(currentConnectionId, _stringLocalizer[Responses.Hub.VolunteerDisconnected].Value);
                }

                // Remove the volunteer from the cache and remove all his occurencies from
                // all the help requests
                await _hubCacheService.RemoveVolunteerConnectionAsync(currentConnectionId);
                return;
            }

            // It is a visually impaired person

            // He might disconnect while he is in a call so notify volunteer to end the call
            (var vIInACall, var volunteerConnectionId) = await _hubCacheService.IsInACallAsync(currentConnectionId);

            if (vIInACall)
            {
                await _hubCacheService.RemoveCallAsync(currentConnectionId);

                await Clients.Client(volunteerConnectionId).VisuallyImpaireDisconnected(currentConnectionId, _stringLocalizer[Responses.Hub.VisuallyImpairedDisconnected].Value);
            }

            // Or while he is requesting for help so notify elected volunteers that this person has disconnected
            else if (await _hubCacheService.HasRequestForHelpAsync(currentConnectionId))
            {
                // Get the elected volunteers
                var electedVolunteers = await _hubCacheService.GetElectedVolunteersForConnectionAsync(currentConnectionId);

                // Notify them that the visually impaired person has disconnected
                // Pass the connectionId of the VI so that they delete it from the UI
                await Clients.Clients(electedVolunteers).VisuallyImpaireDisconnected(currentConnectionId, _stringLocalizer[Responses.Hub.VisuallyImpairedDisconnected].Value);
            }

            // Remove the visually impaired connection from the cache and removes all his requests if there are any
            await _hubCacheService.RemoveVIConnectionAsync(currentConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Handles when a client (visually impaired) requests help from the volunteers
        /// </summary>
        public async Task<bool> RequestVolunteerHelp()
        {
            var currentConnectionId = Context.ConnectionId;
            var isVolunteer = !_loggedInUserSerivce.UserId.IsNullOrEmptyOrWhiteSpaceSafe();

            // If the current user is a volunteer
            if (isVolunteer)
            {
                // Send an error message
                await Clients.Caller.Error(_stringLocalizer[Responses.Hub.NotAVisualImpaired].Value);
                return false;
            }

            // If the current user is already in a call
            (var inACall, _) = await _hubCacheService.IsInACallAsync(currentConnectionId);

            if (inACall)
            {
                // Send an error message
                await Clients.Caller.Error(_stringLocalizer[Responses.Hub.AlreadyInACall].Value);
                return false;
            }

            // If there is currently a request for volunteers that is going on right now
            if (await _hubCacheService.HasRequestForHelpAsync(currentConnectionId))
            {
                // Send an error message
                await Clients.Caller.Error(_stringLocalizer[Responses.Hub.AlreadyRequestedForHelp].Value);
                return false;
            }

            // Get the most likely people to answer the visually impaired and track those volunteers for later processing
            var electedVolunteersConnectionIds = await _hubCacheService.GetMostLikelyToAnswerAndAddHelpRequestAsync(currentConnectionId);

            // Send to those volunteers that a new visually impaired is needed help
            await Clients.Clients(electedVolunteersConnectionIds).NewVINeedingHelp(currentConnectionId);

            return true;
        }

        /// <summary>
        /// Handles when a volunteer accetps a help request initiated by the Visually impaired
        /// </summary>
        /// <param name="targetConnectionId">Target connection the accept the help request of</param>
        [Authorize]
        public async Task<bool> AcceptCall(string targetConnectionId)
        {
            // Check if the target user is a visually impaired user or not
            if (await _hubCacheService.IsVolunteerAsync(targetConnectionId))
            {
                // Send an error message
                await Clients.Caller.Error(_stringLocalizer[Responses.Hub.CantAcceptCall].Value);
                return false;
            }

            // Check if the Visually impaired still exists and is connected
            if (!await _hubCacheService.StillExistsAsync(targetConnectionId))
            {
                // Notify the volunteer that the visually impaired has disconnected
                await Clients.Caller.VisuallyImpaireDisconnected(targetConnectionId, _stringLocalizer[Responses.Hub.VisuallyImpairedDisconnected].Value);
                return false;
            }

            /*
             * Check if the visually impaired is in a call already which happens when concurrent events 
             * occur where someone else accepted the call and updates to the UI for others did not arrive yet
            */
            (var inACall, _) = await _hubCacheService.IsInACallAsync(targetConnectionId);

            if (inACall)
            {
                // Notify the volunteer that the visually impaired has got accepted already by another volunteer
                await Clients.Caller.AlreadyInACall(_stringLocalizer[Responses.Hub.AlreadyGotAcceptedForACall].Value);
                return false;
            }

            // Check if the visually impaired person has already hung up or cancelled his request for help
            if (!await _hubCacheService.HasRequestForHelpAsync(targetConnectionId))
            {
                await Clients.Caller.CancelledHelpRequest(targetConnectionId, _stringLocalizer[Responses.Hub.VICancelledHelpRequest].Value);
                return false;
            }

            // Get all previously elected volunteers
            var previousElectedVolunteers = await _hubCacheService.GetElectedVolunteersForConnectionAsync(targetConnectionId);

            // Notify previously elected volunteers that the VI has got accepted by a volunteer
            await Clients.Clients(previousElectedVolunteers).VIAcceptedByVolunteer(targetConnectionId);

            // Add a new active call between the target VI and the accepter volunteer
            await _hubCacheService.AddNewActiveCallAndDeleteHelpRequestAsync(targetConnectionId, Context.ConnectionId);

            // Tell the visually impaired that a volunteer accpeted the call
            await Clients.Client(targetConnectionId).VolunteerAcceptedCall(Context.ConnectionId);

            return true;
        }

        /// <summary>
        /// Handles when a user hangs up while requesting help or during an active call
        /// </summary>
        public async Task<bool> Hangup()
        {
            var currentConnectionId = Context.ConnectionId;

            if (!await _hubCacheService.StillExistsAsync(currentConnectionId))
                return false;

            var isVolunteer = !_loggedInUserSerivce.UserId.IsNullOrEmptyOrWhiteSpaceSafe();

            // If it was a volunteer
            if (isVolunteer)
            {
                // If he is in a call (which he should be to be able to call this method)
                // If not then the VI must have pressed hung up and updates to the UI haven't arrive yet for the
                // volunteer for him not being able to hung up
                (var volunteerInACall, var vIConnectionId) = await _hubCacheService.IsInACallAsync(currentConnectionId);

                if (volunteerInACall)
                {
                    // Inform the VI that the volunteer has hung up
                    await Clients.Client(vIConnectionId).VolunteerHasHungup();

                    // Remove the call from the cache
                    await _hubCacheService.RemoveCallAsync(vIConnectionId);
                }

                return true;
            }

            // It is a visually impaired person

            // If he is in a call
            (var vIInACall, var volunteerConnectionId) = await _hubCacheService.IsInACallAsync(currentConnectionId);

            if (vIInACall)
            {
                await Clients.Client(volunteerConnectionId).VisuallyImpairedHasHungUp();

                await _hubCacheService.RemoveCallAsync(currentConnectionId);

                return true;
            }

            // Or he has an active request for help

            // Get the elected volunteers
            var electedVolunteers = await _hubCacheService.GetElectedVolunteersForConnectionAsync(currentConnectionId);

            // Notify them that the visually impaired person has cancelled the help request
            // Pass the connectionId of the VI so that they delete it from the UI
            if (electedVolunteers != null && electedVolunteers.Any())
            {
                await Clients.Clients(electedVolunteers).CancelledHelpRequest(currentConnectionId, _stringLocalizer[Responses.Hub.VICancelledHelpRequest].Value);

                // Remove the help request initiated by the VI
                await _hubCacheService.RemoveHelpRequestAsync(currentConnectionId);
            }

            return true;
        }

        /// <summary>
        /// Handles when the current user sends a signal (data) to the target connection
        /// </summary>
        /// <param name="signal">Data to be send to the target connection</param>
        /// <param name="targetConnectionId">The target connection to send the signal to</param>
        public async Task<bool> SendSignal(string signal)
        {
            //    // If the target user disconnected
            //    if (!await _hubCacheService.StillExistsAsync(targetConnectionId))
            //    {
            //        // If it is a visually impaired person
            //        if (_loggedInUserSerivce.UserId.IsNullOrEmptyOrWhiteSpaceSafe())
            //            // Send to the VI that the volunteer has disconnected
            //            await Clients.Client(Context.ConnectionId).VolunteerDisconnected(targetConnectionId, _stringLocalizer[Responses.Hub.VolunteerDisconnected].Value);
            //        // Else it is a volunteer
            //        else
            //            // Send to the volunteer that the VI person has disconnected
            //            await Clients.Client(Context.ConnectionId).VisuallyImpaireDisconnected(targetConnectionId, _stringLocalizer[Responses.Hub.VisuallyImpairedDisconnected].Value);

            //        return false;
            //    }

            (var isInACall, var otherConnectionId) = await _hubCacheService.IsInACallAsync(Context.ConnectionId);

            if (!isInACall)
            {
                await Clients.Caller.Error(_stringLocalizer[Responses.Hub.NotInACall].Value);
                return false;
            }

            // Then send the signal to the target user
            await Clients.Client(otherConnectionId).RecievedSignal(signal);

            return true;
        }
    }

    public interface IServerEvents
    {
        Task ConnectedSuccessfully(string connectionId);
        Task RecievedSignal(string signal);
        Task NewVINeedingHelp(string connectionId);
        Task Error(string errorMessage);
        Task VolunteerDisconnected(string connectionId, string message);
        Task VisuallyImpaireDisconnected(string connectionId, string message);
        Task AlreadyInACall(string message);
        Task CancelledHelpRequest(string connectionId, string message);
        Task VIAcceptedByVolunteer(string connectionId);
        Task VolunteerAcceptedCall(string volunteerConnectionId);
        Task VolunteerHasHungup();
        Task VisuallyImpairedHasHungUp();
    }
}
