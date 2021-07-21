using Oyooni.Server.Data.BusinessModels;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Cache
{
    /// <summary>
    /// Represents the hub cache service contract
    /// </summary>
    public interface IHubCacheService
    {
        /// <summary>
        /// Adds the connection to the cache store
        /// </summary>
        Task<bool> SetConnectionAsync(string connectionId, bool isVIUser = false, AppUser user = null, CancellationToken token = default);

        /// <summary>
        /// Retrieves the volunteers that were elected for the VI user
        /// </summary>
        Task<string[]> GetElectedVolunteersForConnectionAsync(string viConnectionId, CancellationToken token = default);

        /// <summary>
        /// Checks if the passed connection identifier is for a volunteer
        /// </summary>
        Task<bool> IsVolunteerAsync(string connectionId, CancellationToken token = default);

        /// <summary>
        /// Checks if the passed connection identifier is in a call, if yes it fills the <paramref name="otherConnectionId"/>
        /// with the other connection that is in the call
        /// </summary>
        Task<(bool, string)> IsInACallAsync(string connectionId, CancellationToken token = default);

        /// <summary>
        /// Checks whether the two passed connection identifiers are in a call
        /// </summary>
        Task<bool> AreInACallAsync(string connectionId1, string connectionId2, CancellationToken token = default);

        /// <summary>
        /// Checks if the passed connection identifier exists in the cache
        /// </summary>
        Task<bool> StillExistsAsync(string connectionId, CancellationToken token = default);

        /// <summary>
        /// Retrieves the most likely volunteers to answer a help request and creates a new help request for the 
        /// VI user and sets the elected volunteers to the newly created help request
        /// </summary>
        Task<string[]> GetMostLikelyToAnswerAndAddHelpRequestAsync(string vIConnectionId, CancellationToken token = default);

        /// <summary>
        /// Removes the volunteer connection from cache while also removing his connections to existing help requests
        /// </summary>
        Task<bool> RemoveVolunteerConnectionAsync(string volunteerConnectionId, CancellationToken token = default);

        /// <summary>
        /// Removes the VI user connection from the cache and remove his help request if there is one
        /// </summary>
        Task<bool> RemoveVIConnectionAsync(string vIConnectionId, CancellationToken token = default);

        /// <summary>
        /// Checks whether if the VI user has an active help request
        /// </summary>
        Task<bool> HasRequestForHelpAsync(string vIConnectionId, CancellationToken token = default);

        /// <summary>
        /// Adds a new active call and deletes help request for the VI user
        /// </summary>
        Task<bool> AddNewActiveCallAndDeleteHelpRequestAsync(string vIConnectionId, string volunteerConnectionId, CancellationToken token = default);

        /// <summary>
        /// Remove the call, the VI user is engaged in, from the cache
        /// </summary>
        Task<bool> RemoveCallAsync(string vIConnectionId, CancellationToken token = default);

        /// <summary>
        /// Removes the help request from the cache
        /// </summary>
        Task<bool> RemoveHelpRequestAsync(string vIConnectionId, CancellationToken token = default);
    }
}
