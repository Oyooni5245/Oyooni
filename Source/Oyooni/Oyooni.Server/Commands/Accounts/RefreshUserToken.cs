using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oyooni.Server.Constants;
using Oyooni.Server.Data;
using Oyooni.Server.Data.BusinessModels;
using Oyooni.Server.Dtos.Accounts;
using Oyooni.Server.Exceptions;
using Oyooni.Server.Services.Accounts.TokenProviders;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Commands.Accounts
{
    /// <summary>
    /// Represents a refresh token command containing the request and the handler types
    /// </summary>
    public class RefreshUserToken
    {
        /// <summary>
        /// Represents the request type for the refresh token command
        /// </summary>
        public class Request : IRequest<IAuthToken>
        {
            /// <summary>
            /// The expired jwt token
            /// </summary>
            public string JwtToken { get; }

            /// <summary>
            /// The refresh token used to refresh the jwt token
            /// </summary>
            public string RefreshToken { get; }

            /// <summary>
            /// Constructs a new instance of the <see cref="Request"/> class
            /// </summary>
            public Request(string jwtToken, string refreshToken)
                => (JwtToken, RefreshToken) = (jwtToken, refreshToken);
        }

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, IAuthToken>
        {
            /// <summary>
            /// The validation parameters used to validate the authenticity of the expired jwt token
            /// </summary>
            protected readonly TokenValidationParameters _validationParameters;

            /// <summary>
            /// The provider used to generate auth tokens
            /// </summary>
            protected readonly IAuthorizationTokenProvider _authTokenProvider;

            /// <summary>
            /// Manger used to do operations on the <see cref="AppUser"/> class
            /// </summary>
            protected readonly UserManager<AppUser> _userManager;

            /// <summary>
            /// Application context used to do database operations
            /// </summary>
            protected readonly IApplicationDbContext _context;

            /// <summary>
            /// Constructs a new instance of the <see cref="Handler"/> class
            /// </summary>
            public Handler(TokenValidationParameters validationParameters,
                IAuthorizationTokenProvider authTokenProvider,
                UserManager<AppUser> userManager,
                IApplicationDbContext context)
            {
                _validationParameters = validationParameters;
                _authTokenProvider = authTokenProvider;
                _userManager = userManager;
                _context = context;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<IAuthToken> Handle(Request request, CancellationToken cancellationToken)
            {
                // Get the principal user from the token
                var principal = GetPrincipalFromToken(request.JwtToken);

                // If the token has some errors
                if (principal is null)
                    throw new BadRequestException(Responses.Accounts.InvalidRefresh);

                // Get the jwt id for the principal
                var jti = principal.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                // Get the refresh token
                var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(r => r.Token == request.RefreshToken);

                // If token not found or it is expired or it is invalidated or it has been used before or belongs to a different jwt id
                if (refreshToken is null || DateTime.UtcNow > refreshToken.ExpiryDate ||
                    refreshToken.Invalidated || refreshToken.Used || refreshToken.Jid != jti)
                    throw new BadRequestException(Responses.Accounts.InvalidRefresh);

                // Mark the refresh token as used
                refreshToken.Used = true;

                // Save changes to the db
                await _context.SaveChangesAsync();

                // Get the user according to the id contained in the principal
                var user = await _userManager.FindByIdAsync(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

                // Generate a new auth token for the retrieved user
                return await _authTokenProvider.GenerateAuthTokenForUserAsync(user);
            }

            /// <summary>
            /// Gets a principal user from the jwt token
            /// </summary>
            /// <param name="token">The jwt token to get the principal user from</param>
            protected ClaimsPrincipal GetPrincipalFromToken(string token)
            {
                try
                {
                    // Validate token according to the validation parameters and create a principal user
                    var principal =
                        new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out var validatedToken);

                    // return a principal or null for whether the jwt token has the correct security algorithm
                    return IsJwtWithValidSecurityAlgorithm(validatedToken) ? principal : null;
                }
                catch
                {
                    // Return null in case there is any error
                    return null;
                }
            }

            /// <summary>
            /// Checks if the security token has the correct security algorithm
            /// </summary>
            /// <param name="securityToken">Security token to check</param>
            protected bool IsJwtWithValidSecurityAlgorithm(SecurityToken securityToken)
            {
                // If it is a jwt token and has the correct algorithm
                return securityToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        System.StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
