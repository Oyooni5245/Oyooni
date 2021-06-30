using Oyooni.Server.Data.BusinessModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Cache.InMemory
{
    /// <summary>
    /// Represents an in-memory implementer of the <see cref="IHubCacheService"/> contract
    /// </summary>
    // [Injected(ServiceLifetime.Singleton, typeof(IHubCacheService), ignoreForNow: false)]
    public class InMemoryHubCacheService : IHubCacheService
    {
        /// <summary>
        /// The store of the active help requests
        /// </summary>
        protected ConcurrentDictionary<string, HashSet<string>> _helpRequestsMappings;

        /// <summary>
        /// The store of the connected volunteers
        /// </summary>
        protected ConcurrentDictionary<string, InMemoryHubVolunteer> _volunteers;

        /// <summary>
        /// The store of the connected visually impaired users
        /// </summary>
        protected List<string> _visuallyImpairedUsers;

        /// <summary>
        /// The store of the current active calls
        /// </summary>
        protected List<ActiveCall> _activeCalls;

        /// <summary>
        /// The lock used for the <see cref="_visuallyImpairedUsers"/> store
        /// </summary>
        protected object _lockForVisuallyImpairedList = new object();

        /// <summary>
        /// The lock used for the <see cref="_activeCalls"/> store
        /// </summary>
        protected object _lockForActiveCallsList = new object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public InMemoryHubCacheService()
        {
            // Initialize all stores
            _helpRequestsMappings = new ConcurrentDictionary<string, HashSet<string>>();
            _volunteers = new ConcurrentDictionary<string, InMemoryHubVolunteer>();
            _activeCalls = new List<ActiveCall>();
            _visuallyImpairedUsers = new List<string>();
        }

        /// <summary>
        /// Adds a new active call to the memory and deletes the help requests for the VI user
        /// </summary>
        /// <returns></returns>
        public Task<bool> AddNewActiveCallAndDeleteHelpRequestAsync(string vIConnectionId, string volunteerConnectionId, CancellationToken token = default)
        {
            // Lock the active calls store
            lock (_lockForActiveCallsList)
            {
                // Add a new active call
                _activeCalls.Add(new ActiveCall(volunteerConnectionId, vIConnectionId));
            }

            // Remove all the connections of the volunteer (help requests he is in)
            RemoveVolunteerConnections(volunteerConnectionId);

            // Make the volunteer in a call
            _volunteers[volunteerConnectionId].IsInACall = true;

            // Remove the help request for the VI user
            return Task.FromResult(_helpRequestsMappings.TryRemove(vIConnectionId, out var _));
        }

        /// <summary>
        /// Checks whether the passed connections are in an active call
        /// </summary>
        public Task<bool> AreInACallAsync(string connectionId1, string connectionId2, CancellationToken token = default)
        {
            // Lock the active calls store
            lock (_lockForActiveCallsList)
            {
                // Check if there is an entry in the active calls store where the connections passed are in it both together
                return Task.FromResult(_activeCalls.Any(c => c.VIConnectionId == connectionId1 && c.VolunteerConnectionId == connectionId2
                || c.VIConnectionId == connectionId2 && c.VolunteerConnectionId == connectionId1));
            }
        }

        /// <summary>
        /// Retrieves the elected volunteers for a help request related to a VI user
        /// </summary>
        public Task<string[]> GetElectedVolunteersForConnectionAsync(string vIConnectionId, CancellationToken token = default)
            // Get the volunteers of the VI user's help request
            => Task.FromResult(_helpRequestsMappings.GetValueOrDefault(vIConnectionId)?.ToArray());

        /// <summary>
        /// Gets the most likely people to answer the help request, creates a new help request and added the elected volunteers to
        /// the help request
        /// </summary>
        public Task<string[]> GetMostLikelyToAnswerAndAddHelpRequestAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Get the volunteers that are not currently engaged in an active call
            var mostLikelyVolunteers = _volunteers.Where(pair => !pair.Value.IsInACall).Select(pair => pair.Key).ToArray();

            // Add a new help request with the elected volunteers
            _helpRequestsMappings.TryAdd(vIConnectionId, new HashSet<string>(mostLikelyVolunteers));

            // Return the elected volunteers conneciton identifiers
            return Task.FromResult(mostLikelyVolunteers);
        }

        /// <summary>
        /// Checks whether the VI user has an active help request
        /// </summary>
        public Task<bool> HasRequestForHelpAsync(string vIConnectionId, CancellationToken token = default)
            // Check whether the store of help requests has key of the passed connection identifier
            => Task.FromResult(_helpRequestsMappings.ContainsKey(vIConnectionId));

        /// <summary>
        /// Checks whether the passed connection identifier of a user is in an active call right now
        /// and returns the callee connection identifier if the call exists
        /// </summary>
        public Task<(bool, string)> IsInACallAsync(string connectionId, CancellationToken token = default)
        {
            // Initialize the callee connection identifier
            var otherConnectionId = string.Empty;

            // Lock the active calls store
            lock (_lockForActiveCallsList)
            {
                // Get the active call that the passed connection identifier is engaged in
                var activeCall = _activeCalls.SingleOrDefault(c => c.VIConnectionId == connectionId
                    || c.VolunteerConnectionId == connectionId);

                // If it doesn't exist then just return false
                if (activeCall == null) return Task.FromResult((false, otherConnectionId));

                // Set the callee connection identifier
                otherConnectionId = activeCall.VIConnectionId == connectionId ? activeCall.VolunteerConnectionId : activeCall.VIConnectionId;
            }

            // Return success along with the callee connection identifier
            return Task.FromResult((true, otherConnectionId));
        }

        /// <summary>
        /// Checks whether the passed connection identifier belongs to a volunteer or not
        /// </summary>
        public Task<bool> IsVolunteerAsync(string connectionId, CancellationToken token = default)
            // Check if the volunteers store contains a key of the passed connection identifier
            => Task.FromResult(_volunteers.ContainsKey(connectionId));

        /// <summary>
        /// Removes the call from the calls store
        /// </summary>
        public Task<bool> RemoveCallAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Lock the active calls store
            lock (_lockForActiveCallsList)
            {
                // Remove the entry related to the passed connection identifier
                return Task.FromResult(_activeCalls.RemoveAll(c => c.VIConnectionId == vIConnectionId) > 0);
            }
        }

        /// <summary>
        /// Remove the related help request for the passed VI user connection identifier
        /// </summary>
        public Task<bool> RemoveHelpRequestAsync(string vIConnectionId, CancellationToken token = default)
            // Remove the entry in the help requests store with the key equal to the passed VI user connection identifier
            => Task.FromResult(_helpRequestsMappings.TryRemove(vIConnectionId, out var _));

        /// <summary>
        /// Remove the VI user from the local memory
        /// </summary>
        public async Task<bool> RemoveVIConnectionAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Lock the VI users store
            lock (_lockForVisuallyImpairedList)
            {
                // Remove the VI usser
                _visuallyImpairedUsers.Remove(vIConnectionId);
            }

            // Remove the related help requests
            return await RemoveHelpRequestAsync(vIConnectionId, token);
        }

        /// <summary>
        /// Remove the volunteer from the local memory
        /// </summary>
        public Task<bool> RemoveVolunteerConnectionAsync(string volunteerConnectionId, CancellationToken token = default)
        {
            // Remove the volunteers connections
            RemoveVolunteerConnections(volunteerConnectionId);

            // Remove the volunteer from the volunteers store
            return Task.FromResult(_volunteers.TryRemove(volunteerConnectionId, out var _));
        }

        /// <summary>
        /// Stores the connection identifier in the local memory
        /// </summary>
        public Task<bool> SetConnectionAsync(string connectionId, bool isVIUser = false, AppUser user = null, CancellationToken token = default)
        {
            // If it is a VI user
            if (isVIUser)
            {
                // Lock the VI store and store the VI user
                lock (_lockForVisuallyImpairedList) { _visuallyImpairedUsers.Add(connectionId); }

                // Return true
                return Task.FromResult(true);
            }

            // Return the successfull creation of the volunteer and storage in the local store
            return Task.FromResult(_volunteers.TryAdd(connectionId, new InMemoryHubVolunteer
            {
                Id = user.Id,
                FullName = user.FullName,
                AvailableTimes = user.AvailableTimes
            }));
        }

        /// <summary>
        /// Checks if the passed connection still exists
        /// </summary>
        public Task<bool> StillExistsAsync(string connectionId, CancellationToken token = default)
            // Check if the passed connection identifier exists in the volunteers or the VI stores
            => Task.FromResult(_volunteers.ContainsKey(connectionId) || _visuallyImpairedUsers.Contains(connectionId));

        /// <summary>
        /// Removes the volunteer connections
        /// </summary>
        protected void RemoveVolunteerConnections(string volunteerConnectionId)
        {
            // Loop over all help requests and remove the volunteer connection identifer if it exists
            foreach (var pair in _helpRequestsMappings)
                pair.Value.RemoveWhere(v => v == volunteerConnectionId);
        }
    }
}
