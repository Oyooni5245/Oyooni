using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oyooni.Server.Data.BusinessModels;

namespace Oyooni.Server.Data.Mappings.Builders
{
    // <summary>
    /// Represents a builder class for the <see cref="AvailableTime"/> business model
    /// </summary>
    public class AvailableTimeBuilder : EntityBuilder<AvailableTime>
    {
        /// <summary>
        /// Configures the builder according to rules defined for <see cref="AvailableTime"/> model
        /// </summary>
        /// <param name="builder">Builder to use for configuration</param>
        public override void Configure(EntityTypeBuilder<AvailableTime> builder)
        {
            // Set Id as key
            builder.HasKey(a => a.Id);
            
            // Set the one-to-many relation with the user
            builder.HasOne(a => a.User)
                .WithMany(u => u.AvailableTimes)
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore mapping the DayOfWeek property as it is merely a helper
            builder.Ignore(a => a.DayOfWeek);
        }
    }
}
