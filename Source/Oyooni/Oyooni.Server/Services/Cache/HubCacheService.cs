using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using Oyooni.Server.Data.BusinessModels;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Cache
{
    [Injected(ServiceLifetime.Singleton, typeof(IHubCacheService), ignoreForNow: false)]
    public class HubCacheService : IHubCacheService
    {
        protected ConcurrentDictionary<string, HashSet<string>> _helpRequestsMappings;
        protected ConcurrentDictionary<string, HubCacheUser> _volunteers;
        protected List<string> _visuallyImpairedUsers;
        protected List<ActiveCall> _activeCalls;
        protected object _lockForVisuallyImpairedList = new object();
        protected object _lockForActiveCallsList = new object();

        public HubCacheService()
        {
            _helpRequestsMappings = new ConcurrentDictionary<string, HashSet<string>>();
            _volunteers = new ConcurrentDictionary<string, HubCacheUser>();
            _activeCalls = new List<ActiveCall>();
            _visuallyImpairedUsers = new List<string>();
        }

        public Task<bool> AddNewActiveCallAndDeleteHelpRequestAsync(string vIConnectionId, string volunteerConnectionId, CancellationToken token = default)
        {
            lock (_lockForActiveCallsList)
            {
                _activeCalls.Add(new ActiveCall
                {
                    VIConnectionId = vIConnectionId,
                    VolunteerConnectionId = volunteerConnectionId
                });
            }

            RemoveVolunteerConnections(volunteerConnectionId);

            return Task.FromResult(_helpRequestsMappings.TryRemove(vIConnectionId, out var _));
        }

        public Task<bool> AreInACallAsync(string connectionId1, string connectionId2, CancellationToken token = default)
        {
            lock (_lockForActiveCallsList)
            {
                return Task.FromResult(_activeCalls.Any(c => c.VIConnectionId == connectionId1 && c.VolunteerConnectionId == connectionId2
                || c.VIConnectionId == connectionId2 && c.VolunteerConnectionId == connectionId1));
            }
        }

        public Task<string[]> GetElectedVolunteersForConnectionAsync(string vIConnectionId, CancellationToken token = default)
            => Task.FromResult(_helpRequestsMappings.GetValueOrDefault(vIConnectionId)?.ToArray());

        public Task<string[]> GetMostLikelyToAnswerAndSetAsync(string vIConnectionId, CancellationToken token = default)
        {
            var mostLikelyVolunteers = _volunteers.Where(pair => !pair.Value.IsInACall).Select(pair => pair.Key).ToArray();

            _helpRequestsMappings.TryAdd(vIConnectionId, new HashSet<string>(mostLikelyVolunteers));

            return Task.FromResult(mostLikelyVolunteers);
        }

        public Task<bool> HasRequestForHelpAsync(string vIConnectionId, CancellationToken token = default)
            => Task.FromResult(_helpRequestsMappings.ContainsKey(vIConnectionId));

        public Task<bool> IsInACallAsync(string connectionId, out string otherConnectionId, CancellationToken token = default)
        {
            otherConnectionId = string.Empty;

            lock (_lockForActiveCallsList)
            {
                var activeCall = _activeCalls.SingleOrDefault(c => c.VIConnectionId == connectionId
                    || c.VolunteerConnectionId == connectionId);

                if (activeCall == null) return Task.FromResult(false);

                otherConnectionId = activeCall.VIConnectionId == connectionId ? activeCall.VolunteerConnectionId : activeCall.VIConnectionId;

            }

            return Task.FromResult(true);
        }

        public Task<bool> IsVolunteerAsync(string connectionId, CancellationToken token = default)
            => Task.FromResult(_volunteers.ContainsKey(connectionId));

        public Task<bool> RemoveCallAsync(string vIConnectionId, CancellationToken token = default)
        {
            lock (_lockForActiveCallsList)
            {
                return Task.FromResult(_activeCalls.RemoveAll(c => c.VIConnectionId == vIConnectionId) > 0);
            }
        }

        public Task<bool> RemoveHelpRequestAsync(string vIConnectionId, CancellationToken token = default)
            => Task.FromResult(_helpRequestsMappings.TryRemove(vIConnectionId, out var _));

        public Task<bool> RemoveVIConnectionAsync(string vIConnectionId, CancellationToken token = default)
        {
            lock (_lockForVisuallyImpairedList)
            {
                _visuallyImpairedUsers.Remove(vIConnectionId);
            }

            return Task.FromResult(_helpRequestsMappings.TryRemove(vIConnectionId, out var _));
        }

        public Task<bool> RemoveVolunteerConnectionAsync(string volunteerConnectionId, CancellationToken token = default)
        {
            var value = _volunteers.TryRemove(volunteerConnectionId, out var _);

            RemoveVolunteerConnections(volunteerConnectionId);

            return Task.FromResult(value);
        }

        public Task<bool> SetConnectionAsync(string connectionId, bool isVIUser = false, AppUser user = null, CancellationToken token = default)
        {
            if (isVIUser)
            {
                lock (_lockForVisuallyImpairedList) { _visuallyImpairedUsers.Add(connectionId); }
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(_volunteers.TryAdd(connectionId, new HubCacheUser
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    AvailableTimes = user.AvailableTimes
                }));
            }
        }

        public Task<bool> StillExistsAsync(string connectionId, CancellationToken token = default)
            => Task.FromResult(_volunteers.ContainsKey(connectionId) || _visuallyImpairedUsers.Contains(connectionId));

        protected void RemoveVolunteerConnections(string volunteerConnectionId)
        {
            foreach (var pair in _helpRequestsMappings)
                pair.Value.RemoveWhere(v => v == volunteerConnectionId);
        }
    }
}
