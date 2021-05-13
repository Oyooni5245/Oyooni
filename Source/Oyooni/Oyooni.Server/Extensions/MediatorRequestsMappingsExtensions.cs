using Oyooni.Server.Commands.Accounts;
using Oyooni.Server.Requests.Accounts;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// Extensions to map requests to mediator requests
    /// </summary>
    public static class MediatorRequestsMappingsExtensions
    {
        /// <summary>
        /// Maps <see cref="LoginRequest"/> to <see cref="Login.Request"/>
        /// </summary>
        public static Login.Request ToMediatorRequest(this LoginRequest request)
            => new Login.Request(request.Email, request.Password);

        /// <summary>
        /// Maps <see cref="RefreshTokenRequest"/> to <see cref="RefreshUserToken.Request"/>
        /// </summary>
        public static RefreshUserToken.Request ToMediatorRequest(this RefreshTokenRequest request)
            => new RefreshUserToken.Request(request.Token, request.RefreshToken);

        /// <summary>
        /// Maps <see cref="SignupRequest"/> to <see cref="Signup.Request"/>
        /// </summary>
        public static Signup.Request ToMediatorRequest(this SignupRequest request)
            => new Signup.Request(request.FirstName, request.LastName, request.Email, request.Password);
    }
}
