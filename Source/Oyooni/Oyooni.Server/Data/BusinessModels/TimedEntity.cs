using Oyooni.Server.Data.BusinessModels.Interfaces;
using System;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents a time-trackable entity
    /// </summary>
    public abstract class TimedEntity : BaseEntity, ITimedEntity
    {
        /// <summary>
        /// Default constructoor
        /// </summary>
        public TimedEntity()
        {
            // Set the created and the edit date to the current date
            CreatedDate = EditedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// The creation data of the entity
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The modification data of the entity
        /// </summary>
        public DateTime EditedDate { get; set; }
    }
}
