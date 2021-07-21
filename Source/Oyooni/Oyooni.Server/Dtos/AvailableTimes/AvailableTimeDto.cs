using System;

namespace Oyooni.Server.Dtos.AvailableTimes
{
    /// <summary>
    /// Represents the data transfer object of the Available time data model
    /// </summary>
    public class AvailableTimeDto
    {
        /// <summary>
        /// The available time identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The identifier of the day of the week
        /// </summary>
        public int DayOfWeekId { get; set; }

        /// <summary>
        /// The start time of the available time
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The end time of the available time
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AvailableTimeDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="AvailableTimeDto"/> class using the passed parameters
        /// </summary>
        public AvailableTimeDto(string id, int dayOfWeekId, string from, string to)
            => (Id, DayOfWeekId, From, To) = (id, dayOfWeekId, from, to);
    }
}
