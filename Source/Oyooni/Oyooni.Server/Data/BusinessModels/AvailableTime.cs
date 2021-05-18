using System;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents an available time business entity
    /// </summary>
    public class AvailableTime : AuditableEntity
    {
        /// <summary>
        /// The day of the week identifier
        /// </summary>
        public int DayOfWeekId { get; set; }

        /// <summary>
        /// The day of the week enumeration
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get => (DayOfWeek)DayOfWeekId;
            set => DayOfWeekId = (int)value;
        }

        /// <summary>
        /// The start time
        /// </summary>
        public TimeSpan From { get; set; }

        /// <summary>
        /// The end time
        /// </summary>
        public TimeSpan To { get; set; }
    }
}
