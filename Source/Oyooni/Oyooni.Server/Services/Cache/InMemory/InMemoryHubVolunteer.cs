using Oyooni.Server.Data.BusinessModels;
using System.Collections.Generic;

namespace Oyooni.Server.Services.Cache.InMemory
{
    /// <summary>
    /// Represents an in memory hub volunteer data
    /// </summary>
    public class InMemoryHubVolunteer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InMemoryHubVolunteer()
        {
            AvailableTimes = new HashSet<AvailableTime>();
        }

        /// <summary>
        /// The identifier of the volunteer
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The fullname of the volunteer
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Indicates whether the volunteer is in a call
        /// </summary>
        public bool IsInACall { get; set; }

        /// <summary>
        /// The collection of available times the volunter has previously configured
        /// </summary>
        public ICollection<AvailableTime> AvailableTimes { get; set; }
    }
}
