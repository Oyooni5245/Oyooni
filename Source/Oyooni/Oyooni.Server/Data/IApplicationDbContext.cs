using Microsoft.EntityFrameworkCore;
using Oyooni.Server.Data.BusinessModels;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Data
{
    /// <summary>
    /// Represents the applications database context contract for doing database operations
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// The users db set
        /// </summary>
        DbSet<AppUser> Users { get; set; }

        /// <summary>
        /// The refresh tokens db set
        /// </summary>
        DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Saves changes to the database synchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        int SaveChanges(string userId = null);

        /// <summary>
        /// Saves changes to the database asynchronously
        /// </summary>
        /// <param name="userId">current user identifier</param>
        Task<int> SaveChangesAsync(string userId = null, CancellationToken token = default);
    }
}
