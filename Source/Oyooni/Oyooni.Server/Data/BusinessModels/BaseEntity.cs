using Oyooni.Server.Data.BusinessModels.Interfaces;
using System;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents the base entity
    /// </summary>
    public class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseEntity()
        {
            // Set the identifier to random guid
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The identifier of the entity
        /// </summary>
        public string Id { get; set; }
    }
}
