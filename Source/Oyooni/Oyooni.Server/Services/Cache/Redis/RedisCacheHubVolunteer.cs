using Oyooni.Server.Data.BusinessModels;
using System;
using System.Collections.Generic;

namespace Oyooni.Server.Services.Cache.Redis
{
    /// <summary>
    /// Represents a redis hub volunteer data
    /// </summary>
    public class RedisCacheHubVolunteer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RedisCacheHubVolunteer()
        {
            AvailableTimes = new HashSet<AvailableTime>();
            RelatedHelpRequestsKeys = new HashSet<string>();
        }

        /// <summary>
        /// The identifier of the volunteer's connection 
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// The fullname of the volunteer
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The collection of available times the volunter has previously configured
        /// </summary>
        public ICollection<AvailableTime> AvailableTimes { get; set; }

        /// <summary>
        /// The related help requests the volunteer is related to
        /// </summary>
        public ICollection<string> RelatedHelpRequestsKeys { get; set; }

        /// <summary>
        /// Resets the <see cref="RelatedHelpRequestsKeys"/> collection
        /// </summary>
        public void ResetRelatedHelpRequests()
        {
            RelatedHelpRequestsKeys = new HashSet<string>();
        }
    }
}
