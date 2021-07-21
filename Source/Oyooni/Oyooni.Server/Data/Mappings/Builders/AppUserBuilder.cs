using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oyooni.Server.Data.BusinessModels;

namespace Oyooni.Server.Data.Mappings.Builders
{
    /// <summary>
    /// Represents a builder class for the <see cref="AppUser"/> business model
    /// </summary>
    public class AppUserBuilder : EntityBuilder<AppUser>
    {
        /// <summary>
        /// Configures the builder according to rules defined for <see cref="AppUser"/> model
        /// </summary>
        /// <param name="builder">Builder to use for configuration</param>
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Set Id as the key
            builder.HasKey(a => a.Id);

            // Make FirstName required and with a specific length
            builder.Property(a => a.FirstName).IsRequired().HasMaxLength(32);

            // Make LastName required and with a specific length
            builder.Property(a => a.LastName).HasMaxLength(64);

            // Make the concurrency stamp type as nvarchar (Needed for sqlite)
            builder.Property(a => a.ConcurrencyStamp).HasColumnType("varchar(256)");

            // Ignore mapping the FullName property
            builder.Ignore(a => a.FullName);
        }
    }
}
