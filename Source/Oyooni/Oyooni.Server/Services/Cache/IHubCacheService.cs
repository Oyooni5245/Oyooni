using Oyooni.Server.Data.BusinessModels;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Cache
{
    public interface IHubCacheService
    {
        Task<bool> SetConnectionAsync(string connectionId, bool isVIUser = false, AppUser user = null, CancellationToken token = default);
        Task<string[]> GetElectedVolunteersForConnectionAsync(string viConnectionId, CancellationToken token = default);
        Task<bool> IsVolunteerAsync(string connectionId, CancellationToken token = default);
        Task<bool> IsInACallAsync(string connectionId, out string otherConnectionId, CancellationToken token = default);
        Task<bool> AreInACallAsync(string connectionId1, string connectionId2, CancellationToken token = default);
        Task<bool> StillExistsAsync(string connectionId, CancellationToken token = default);
        Task<string[]> GetMostLikelyToAnswerAndSetAsync(string vIConnectionId, CancellationToken token = default);
        Task<bool> RemoveVolunteerConnectionAsync(string volunteerConnectionId, CancellationToken token = default);
        Task<bool> RemoveVIConnectionAsync(string vIConnectionId, CancellationToken token = default);
        Task<bool> HasRequestForHelpAsync(string vIConnectionId, CancellationToken token = default);
        Task<bool> AddNewActiveCallAndDeleteHelpRequestAsync(string vIConnectionId, string volunteerConnectionId, CancellationToken token = default);
        Task<bool> RemoveCallAsync(string vIConnectionId, CancellationToken token = default);
        Task<bool> RemoveHelpRequestAsync(string vIConnectionId, CancellationToken token = default);
    }
}
