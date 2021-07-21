namespace Oyooni.Server.Services.Cache
{
    /// <summary>
    /// Represents an active call data for the hub cache
    /// </summary>
    public class ActiveCall
    {
        /// <summary>
        /// The volunteer's connection identifier
        /// </summary>
        public string VolunteerConnectionId { get; set; }

        /// <summary>
        /// The VI connection identifier
        /// </summary>
        public string VIConnectionId { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActiveCall() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ActiveCall"/> class using the passed parameters
        /// </summary>
        public ActiveCall(string volunteerConnectionId, string vIConnectionId)
            => (VolunteerConnectionId, VIConnectionId) = (volunteerConnectionId, vIConnectionId);
    }
}
