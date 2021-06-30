using Oyooni.Server.Constants;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Extensions;
using Oyooni.Server.Services.Cache.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Cache
{
    // [Injected(ServiceLifetime.Scoped, typeof(IHubCacheService), ignoreForNow: true)]
    public class RedisCacheHubService : IHubCacheService
    {
        /// <summary>
        /// The redis cache client used to talk to the redis cache server
        /// </summary>
        protected IRedisClientAsync _redisClient;

        /// <summary>
        /// The redis cache clients manager used to create clients
        /// </summary>
        protected readonly IRedisClientsManagerAsync _redisClientsManager;

        /// <summary>
        /// Creates an instance of the <see cref="RedisCacheHubService"/> class using the passed parameters
        /// </summary>
        public RedisCacheHubService(IRedisClientsManagerAsync redisClientsManager)
        {
            _redisClientsManager = redisClientsManager;
        }

        /// <summary>
        /// Adds a new active call and deletes help request for the VI user
        /// </summary>
        public async Task<bool> AddNewActiveCallAndDeleteHelpRequestAsync(string vIConnectionId, string volunteerConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Create a dictionary of the stuff to be added as bulk at the end
            var thingsToAdd = new Dictionary<string, string>();

            // Generate active call keys
            (var key1, var key2) = RedisPrefixes.ActiveCallKeys(vIConnectionId, volunteerConnectionId);

            // Create active call instance and serialize it to json
            var activeCallJson = new ActiveCall(volunteerConnectionId, vIConnectionId).ToJson();

            // Add both active call keys and active call json to the dictionary
            thingsToAdd.Add(key1, activeCallJson);
            thingsToAdd.Add(key2, activeCallJson);

            // Get a not-in-a-call volunteer key
            var volunteerNotInCallKey = RedisPrefixes.VolunteerKey(volunteerConnectionId, false);

            // Get the volunteer
            var volunteer = await _redisClient.GetAndSerializeValueAsync<RedisCacheHubVolunteer>
                (volunteerNotInCallKey, token);

            // Return if it does not exist
            if (volunteer == null) return false;

            // Remove all the help requests that the volunteer is related to
            await RemoveVolunteerConnections(volunteerNotInCallKey, volunteer.RelatedHelpRequestsKeys, token);

            // Set the related request to null
            volunteer.ResetRelatedHelpRequests();

            // Get a in-a-call volunteer key
            var volunteerInCallKey = RedisPrefixes.VolunteerKey(volunteerConnectionId, true);

            // Remove the not-in-a-call volunteer key
            await _redisClient.RemoveAsync(volunteerNotInCallKey, token);

            // Add the volunteer in a call key to the dictionary
            thingsToAdd.Add(volunteerInCallKey, volunteer.ToJson());

            // Set all values gathered
            await _redisClient.SetValuesAsync(thingsToAdd, token);

            return true;
        }

        /// <summary>
        /// Checks whether the two passed connection identifiers are in a call
        /// </summary>
        public async Task<bool> AreInACallAsync(string connectionId1, string connectionId2, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Generate active call keys
            (var key1, var key2) = RedisPrefixes.ActiveCallKeys(connectionId1, connectionId2);

            // Check if both keys exist
            return await _redisClient.ContainsKeyAsync(key1, token) && await _redisClient.ContainsKeyAsync(key2, token);
        }

        /// <summary>
        /// Retrieves the volunteers that were elected for the VI user
        /// </summary>
        public async Task<string[]> GetElectedVolunteersForConnectionAsync(string viConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Generate the help request key for the VI connection
            var helpRequestKey = RedisPrefixes.HelpRequestKey(viConnectionId);

            // Get the elected volunteers connection ids
            var electedVolunteers = await _redisClient.GetAllItemsFromSetAsync(helpRequestKey, token);

            // Return the elected volunteers or an empty array if no elected volunteers were found
            return electedVolunteers?.Select(ev => RedisPrefixes.ExtractConnectionId(ev))?.ToArray() ?? Array.Empty<string>();
        }

        /// <summary>
        /// Retrieves the most likely volunteers to answer a help request and creates a new help request for the 
        /// VI user and sets the elected volunteers to the newly created help request
        /// </summary>
        public async Task<string[]> GetMostLikelyToAnswerAndAddHelpRequestAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Get all volunteers that are not in a call right now
            var notInCallVolunteersKeys = await _redisClient.SearchKeysAsync(RedisPrefixes.VolunteerNotInCallPattern(), token);

            // If none exist
            if (notInCallVolunteersKeys == null || !notInCallVolunteersKeys.Any()) return Array.Empty<string>();

            // Create a dictionary of the stuff to be added later as a bulk
            var thingsToAdd = new Dictionary<string, string>();

            // Get all volunteers that are elected for the help request
            var volunteers = await _redisClient.GetValuesAsync<RedisCacheHubVolunteer>(notInCallVolunteersKeys.ToList(), token);

            // Create the help request key
            var helpRequestKey = RedisPrefixes.HelpRequestKey(vIConnectionId);

            // Add the help request key to the volunteer related help requests
            foreach (var volunteer in volunteers)
            {
                volunteer.RelatedHelpRequestsKeys.Add(helpRequestKey);
                thingsToAdd.Add(RedisPrefixes.VolunteerKey(volunteer.ConnectionId, inCall: false), volunteer.ToJson());
            }

            // Create help request set and add elected volunteers to the set
            await _redisClient.AddRangeToSetAsync(helpRequestKey, notInCallVolunteersKeys, token);

            // Add the bulk gathered
            await _redisClient.SetValuesAsync(thingsToAdd, token);

            // Return elected volunteers
            return volunteers.Select(v => v.ConnectionId).ToArray();
        }

        /// <summary>
        /// Checks whether if the VI user has an active help request
        /// </summary>
        public async Task<bool> HasRequestForHelpAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Form a help request key for the VI and check if the key exists
            return await _redisClient.ContainsKeyAsync(RedisPrefixes.HelpRequestKey(vIConnectionId), token);
        }

        /// <summary>
        /// Checks if the passed connection identifier is in a call, if yes it fills the <paramref name="otherConnectionId"/>
        /// with the other connection that is in the call
        /// </summary>
        public async Task<(bool, string)> IsInACallAsync(string connectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Initialize the other connection
            var otherConnectionId = string.Empty;

            // Form the active call key for the first connection id
            var activeCallKey = RedisPrefixes.ActiveCallKey(connectionId);

            // Get the active call
            var activeCall = await _redisClient.GetAndSerializeValueAsync<ActiveCall>(activeCallKey, token);

            // Return if not exist
            if (activeCall == null) return (false, otherConnectionId);

            // Set the other connection
            otherConnectionId = activeCall.VIConnectionId == connectionId ? activeCall.VolunteerConnectionId : activeCall.VIConnectionId;

            // Return true
            return (true, otherConnectionId);
        }

        /// <summary>
        /// Checks if the passed connection identifier is for a volunteer
        /// </summary>
        public async Task<bool> IsVolunteerAsync(string connectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Get a not-in-a-call volunteer key
            var volunteerNotInCallKey = RedisPrefixes.VolunteerKey(connectionId, false);

            // Get a in-a-call volunteer key
            var volunteerInCallKey = RedisPrefixes.VolunteerKey(connectionId, true);

            // Return whether either of the keys exist
            return await _redisClient.ContainsKeyAsync(volunteerInCallKey, token) || await _redisClient.ContainsKeyAsync(volunteerNotInCallKey, token);
        }

        /// <summary>
        /// Remove the call, the VI user is engaged in, from the cache
        /// </summary>
        public async Task<bool> RemoveCallAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Form the active call key using the VI connection id
            var activeCallVIKey = RedisPrefixes.ActiveCallKey(vIConnectionId);

            // Get the active call
            var activeCall = await _redisClient.GetAndSerializeValueAsync<ActiveCall>(activeCallVIKey, token);

            // Return if it doesn't exist
            if (activeCall == null) return false;

            // Form the active call key using the volunteer connection id
            var activeCallVolunteerKey = RedisPrefixes.ActiveCallKey(activeCall.VolunteerConnectionId);

            // Remove the active call keys
            await _redisClient.RemoveAllAsync(new List<string> { activeCallVIKey, activeCallVolunteerKey }, token);

            await _redisClient.RenameKeyAsync(RedisPrefixes.VolunteerKey(activeCall.VolunteerConnectionId, inCall: true),
                RedisPrefixes.VolunteerKey(activeCall.VolunteerConnectionId, inCall: false), token);

            return true;
        }

        /// <summary>
        /// Removes the help request from the cache
        /// </summary>
        public async Task<bool> RemoveHelpRequestAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Form the help request key
            var helpRequestKey = RedisPrefixes.HelpRequestKey(vIConnectionId);

            // Create a dictionary to add stuff in bulk later
            var thingsToAdd = new Dictionary<string, string>();

            // Remove help request key for volunteers
            await RemoveHelpRequestKeyForVolunteersInHelpRequest(helpRequestKey, thingsToAdd, token);

            // Remove the help request
            await _redisClient.RemoveAsync(helpRequestKey, token);

            // Set the values to be added (edited as well, since addition of an existing value is like an overwrite for redis)
            await _redisClient.SetValuesAsync(thingsToAdd, token);

            return true;
        }

        /// <summary>
        /// Removes the VI user connection from the cache and remove his help request if there is one
        /// </summary>
        public async Task<bool> RemoveVIConnectionAsync(string vIConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Form VI key
            var vIKey = RedisPrefixes.VIKey(vIConnectionId);

            // Remove the VI key
            await _redisClient.RemoveAsync(vIKey, token);

            // Create things to be added dictionary to be added later as a bulk
            var thingsToAdd = new Dictionary<string, string>();

            var helpRequestKey = RedisPrefixes.HelpRequestKey(vIConnectionId);

            if (await _redisClient.ContainsKeyAsync(helpRequestKey, token))
            {
                // Remove help request key for volunteers
                await RemoveHelpRequestKeyForVolunteersInHelpRequest(helpRequestKey, thingsToAdd, token);

                // Remove a help request if there is any for the VI user
                await _redisClient.RemoveAsync(helpRequestKey, token);
            }

            // Set the values to be added
            await _redisClient.SetValuesAsync(thingsToAdd, token);

            return true;
        }

        /// <summary>
        /// Removes the volunteer connection from cache while also removing his connections to existing help requests
        /// </summary>
        public async Task<bool> RemoveVolunteerConnectionAsync(string volunteerConnectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Form both in call and not in call volunteer keys
            var volunteerInCallKey = RedisPrefixes.VolunteerKey(volunteerConnectionId, true);
            var volunteerNotInCallKey = RedisPrefixes.VolunteerKey(volunteerConnectionId, false);

            var keyToRemove = volunteerInCallKey;

            // Get the in-call volunteer and if not exist get the not in call version
            var volunteer = await _redisClient.GetAndSerializeValueAsync<RedisCacheHubVolunteer>(volunteerInCallKey, token: token);

            if (volunteer == null)
            {
                volunteer = await _redisClient.GetAndSerializeValueAsync<RedisCacheHubVolunteer>(volunteerNotInCallKey, token: token);
                keyToRemove = volunteerNotInCallKey;
            }

            // Remove the volunteer from the help requets he is in
            await RemoveVolunteerConnections(keyToRemove, volunteer.RelatedHelpRequestsKeys, token);

            // Remove volunteer
            await _redisClient.RemoveAsync(keyToRemove, token);

            return true;
        }

        /// <summary>
        /// Adds the connection to the cache store
        /// </summary>
        public async Task<bool> SetConnectionAsync(string connectionId, bool isVIUser = false, AppUser user = null, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // if it is a VI user
            if (isVIUser)
            {
                // Save the VI
                await _redisClient.SetValueAsync(RedisPrefixes.VIKey(connectionId), RedisPrefixes.VI, token);
            }
            else
            {
                // Create the volunteer redis cache instance and convert to json
                var volunteerJson = new RedisCacheHubVolunteer
                {
                    ConnectionId = connectionId,
                    FullName = user.FullName,
                    AvailableTimes = user.AvailableTimes
                }.ToJson();

                // Save the volunteer with the appropriate key
                await _redisClient.SetValueAsync(RedisPrefixes.VolunteerKey(connectionId, false), volunteerJson, token);
            }

            return true;
        }

        /// <summary>
        /// Checks if the passed connection identifier exists in the cache
        /// </summary>
        public async Task<bool> StillExistsAsync(string connectionId, CancellationToken token = default)
        {
            // Connect and get a client
            _redisClient = await ConnectARedisClient(token);

            // Get VI key and both versions of volunteer keys
            var vIKey = RedisPrefixes.VIKey(connectionId);
            var volunteerInCallKey = RedisPrefixes.VolunteerKey(connectionId, true);
            var volunteerNotInCallKey = RedisPrefixes.VolunteerKey(connectionId, false);

            // Check if any of the keys exist
            return await _redisClient.ContainsKeyAsync(vIKey, token) ||
                await _redisClient.ContainsKeyAsync(volunteerInCallKey, token) || await _redisClient.ContainsKeyAsync(volunteerNotInCallKey, token);
        }


        /// <summary>
        /// Removes the volunteer's key from the related help requests
        /// </summary>
        /// <returns></returns>
        protected async Task RemoveVolunteerConnections(string volunteerKey, ICollection<string> relatedHelpRequestsKeys, CancellationToken token = default)
        {
            // Loop over all related help requests and remove volunteer connection id from the list
            foreach (var relatedHelpRequestKey in relatedHelpRequestsKeys)
                await _redisClient.RemoveItemFromSetAsync(relatedHelpRequestKey, volunteerKey, token);
        }

        /// <summary>
        /// Removes the help request key from the related help requests of the volunteers
        /// </summary>
        protected async Task RemoveHelpRequestKeyForVolunteersInHelpRequest(string helpRequestKey, Dictionary<string, string> thingsToAdd, CancellationToken token = default)
        {
            // Get the volunteers connection ids related to the help request
            var helpRequestVolunteersKeys = await _redisClient.GetAllItemsFromSetAsync(helpRequestKey, token);

            // Get all elected volunteers
            var electedVolunteers = await _redisClient.GetValuesAsync<RedisCacheHubVolunteer>(helpRequestVolunteersKeys.ToList(), token);

            if (electedVolunteers.Any())
                // Remove the request key from the elected volunteers
                foreach (var volunteer in electedVolunteers)
                {
                    volunteer.RelatedHelpRequestsKeys.Remove(helpRequestKey);
                    thingsToAdd.Add(RedisPrefixes.VolunteerKey(volunteer.ConnectionId, false), volunteer.ToJson());
                }
        }

        /// <summary>
        /// Creates and returns a new redis client
        /// </summary>
        protected async Task<IRedisClientAsync> ConnectARedisClient(CancellationToken token = default)
            => await _redisClientsManager.GetClientAsync(token);
    }
}
