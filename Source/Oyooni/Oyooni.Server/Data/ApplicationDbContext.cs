using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Data.BusinessModels.Interfaces;
using Oyooni.Server.Data.Mappings;
using Oyooni.Server.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Data
{
    /// <summary>
    /// Represents the applications database context class for doing database operations
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
    {
        /// <summary>
        /// Constructs an instance of the <see cref="ApplicationDbContext"/> class using the passed database options
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// The refresh tokens dbset
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// The available times db set
        /// </summary>
        public DbSet<AvailableTime> AvailableTimes { get; set; }

        /// <summary>
        /// Handling when creating the models
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply base class handling
            base.OnModelCreating(builder);

            // Apply all builders and configurations
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IEntityBuilder).IsAssignableFrom(type) &&
                               !type.IsAbstract && !type.IsInterface)
                .ToList().ForEach(type =>
                {
                    ((IEntityBuilder)Activator.CreateInstance(type))
                        .ApplyConfiguration(builder);
                });

            // Seed data
            builder.SeedData();
        }

        /// <summary>
        /// Saves changes to the database synchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        public int SaveChanges(string userId = null)
        {
            // Manage added or changed entities
            ManageEntitiesInChangeTracker(userId);

            // Apply base SaveChanges
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves changes to the database asynchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        public Task<int> SaveChangesAsync(string userId = null, CancellationToken token = default)
        {
            // Manage added or changed entities
            ManageEntitiesInChangeTracker(userId);

            // Apply base SaveChangesAsync
            return base.SaveChangesAsync(token);
        }

        /// <summary>
        /// Manages added or modified entities from the change tracker
        /// </summary>
        protected void ManageEntitiesInChangeTracker(string userId)
        {
            // Get the entries from the change tracker
            var entries = ChangeTracker.Entries();

            // Loop over all entities
            foreach (var entry in entries)
            {
                // Get the state of the entry
                var entryState = entry.State;

                // Get the actual entity of the entry
                var entity = entry.Entity;

                // Check the state
                switch (entryState)
                {
                    // If added
                    case EntityState.Added:
                        // If there is a passed user id and the entity is a user-related entity
                        if (!string.IsNullOrEmpty(userId) && entity is IUserRelatedEntity userRelatedEntityAdded)
                        {
                            // Set the userId to the passed user id
                            userRelatedEntityAdded.UserId = userId;
                        }
                        break;

                    // If modified
                    case EntityState.Modified:

                        // If the entity is a timed entity
                        if (entity is ITimedEntity timedEntityEdited)
                        {
                            // Set the modified entity to utc now
                            timedEntityEdited.EditedDate = DateTime.UtcNow;
                        }
                        break;
                }
            }
        }
    }
}
