using Oyooni.Server.Data.BusinessModels;
using System.Collections.Generic;

namespace Oyooni.Server.Services.Cache
{
    public class HubCacheUser
    {
        public HubCacheUser()
        {
            AvailableTimes = new HashSet<AvailableTime>();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public bool IsInACall { get; set; }
        public ICollection<AvailableTime> AvailableTimes { get; set; }
    }
}
