namespace Oyooni.Server.Data.BusinessModels.Interfaces
{
    /// <summary>
    /// Represents a user-related entiy contract
    /// </summary>
    public interface IUserRelatedEntity
    {
        /// <summary>
        /// The related user identifier
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// The related user instance
        /// </summary>
        AppUser User { get; set; }
    }
}
