using System;

namespace Oyooni.Server.Data.BusinessModels.Interfaces
{
    /// <summary>
    /// Represents a time-trackable entity contract
    /// </summary>
    public interface ITimedEntity
    {
        /// <summary>
        /// The creation data of the entity
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// The modification data of the entity
        /// </summary>
        DateTime EditedDate { get; set; }
    }
}
