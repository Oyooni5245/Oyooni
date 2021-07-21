using System;
using Oyooni.Server.Converters;

namespace Oyooni.Server.Requests.AvailableTimes
{
    /// <summary>
    /// Represents the request for adding a new available time
    /// </summary>
    public class AddAvailableTimeRequest
    {
        /// <summary>
        /// The day of the week idenifier
        /// </summary>
        public int DayOfWeekId { get; set; }

        /// <summary>
        /// The start time of the available time
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan From { get; set; }

        /// <summary>
        /// The end time of the available time
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan To { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddAvailableTimeRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="AddAvailableTimeRequest"/> class using the passed parameters
        /// </summary>
        public AddAvailableTimeRequest(int dayOfWeekId, TimeSpan from, TimeSpan to)
            => (DayOfWeekId, From, To) = (dayOfWeekId, from, to);
    }
}
