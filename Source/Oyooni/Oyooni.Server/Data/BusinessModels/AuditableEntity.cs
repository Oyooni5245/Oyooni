using Oyooni.Server.Data.BusinessModels.Interfaces;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents an auditable user entity
    /// </summary>
    public abstract class AuditableEntity : TimedEntity, IUserRelatedEntity
    {
        /// <summary>
        /// The related user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The related user instance
        /// </summary>
        public virtual AppUser User { get; set; }

        /// <summary>
        /// Checks whether the current object belongs to the passed user identifier
        /// </summary>
        public bool BelongsTo(string userId) => UserId == userId;
    }
}
