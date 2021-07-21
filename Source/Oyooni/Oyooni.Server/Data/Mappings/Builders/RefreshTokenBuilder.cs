using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oyooni.Server.Data.BusinessModels;

namespace Oyooni.Server.Data.Mappings.Builders
{
    /// <summary>
    /// Represents a builder class for the <see cref="RefreshToken"/> business model
    /// </summary>
    public class RefreshTokenBuilder : EntityBuilder<RefreshToken>
    {
        /// <summary>
        /// Configures the builder according to rules defined for <see cref="RefreshToken"/> model
        /// </summary>
        /// <param name="builder">Builder to use for configuration</param>
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Set Id as the key
            builder.HasKey(r => r.Id);

            // Make the token property required and having a max lengh of 128 characters
            builder.Property(r => r.Token).HasMaxLength(128).IsRequired();

            // Make the Jid property required and having a max lengh of 128 characters
            builder.Property(r => r.Jid).HasMaxLength(128).IsRequired();

            // Configure the one-to-many relation with the AppUser class
            builder.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
