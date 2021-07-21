using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Oyooni.Server.Attributes;
using Oyooni.Server.Data.BusinessModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.Accounts
{
    /// <summary>
    /// Represents a service contract for the logged in user
    /// </summary>
    public interface ILoggedInUserService
    {
        /// <summary>
        /// The current user identifier
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// The current user instance
        /// </summary>
        /// <returns></returns>
        Task<AppUser> GetCurrentUser();

        /// <summary>
        /// The claims principal of the current user
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }

    /// <summary>
    /// Represents a service class for the logged in user
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(ILoggedInUserService))]
    public class LoggedInUserService : ILoggedInUserService
    {
        /// <summary>
        /// The http accessor instance
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The manager used to do operations on the <see cref="AppUser"/>
        /// </summary>
        protected readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructs a new instance of the <see cref="LoggedInUserService"/> class using the passed parameters
        /// </summary>
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager)
        {
            // Set the http accesssor
            _httpContextAccessor = httpContextAccessor;

            // Set the user manager
            _userManager = userManager;

            // Get the http context
            var httpContext = _httpContextAccessor.HttpContext;

            // Get the user identifier if it is there
            UserId = httpContext.User
                ?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Get the claims principal if it is there
            ClaimsPrincipal = httpContext.User;
        }

        /// <summary>
        /// The current user identifier
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// The current user claims principal
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// Returns the current user instance
        /// </summary>
        public async Task<AppUser> GetCurrentUser()
            => string.IsNullOrEmpty(UserId) ? null : await _userManager.FindByIdAsync(UserId);
    }
}
