namespace Oyooni.Server.Data.BusinessModels.Interfaces
{
    /// <summary>
    /// Represents a base entity contract
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// The identifier of the entity
        /// </summary>
        string Id { get; set; }
    }
}
