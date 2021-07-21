using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Oyooni.Server.Data.Mappings
{
    /// <summary>
    /// Represents an entity builder class for building mappings of business models
    /// </summary>
    /// <typeparam name="T">The type of class to build the mappings for</typeparam>
    public abstract class EntityBuilder<T> : IEntityBuilder, IEntityTypeConfiguration<T> where T : class
    {
        /// <summary>
        /// Applies the configurations for the <see cref="T"/> class
        /// </summary>
        /// <param name="builder">Builder to be used</param>
        public void ApplyConfiguration(ModelBuilder builder)
        {
            // Configure the entity
            Configure(builder.Entity<T>());
        }

        /// <summary>
        /// Method to be used for configuration
        /// </summary>
        public abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
