using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Accounts.TokenProviders
{
    /// <summary>
    /// Represents an authorization token provider contract
    /// </summary>
    public interface IAuthorizationTokenProvider
    {
        /// <summary>
        /// Generates an <see cref="IAuthToken"/> for the passed user
        /// </summary>
        Task<IAuthToken> GenerateAuthTokenForUserAsync(AppUser user, CancellationToken token = default);
    }
}
