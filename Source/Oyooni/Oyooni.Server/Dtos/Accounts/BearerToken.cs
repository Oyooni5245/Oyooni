namespace Oyooni.Server.Dtos.Accounts
{
    /// <summary>
    /// Represents a bearer auth token
    /// </summary>
    public class BearerToken : IAuthToken
    {
        /// <summary>
        /// The json web token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The related refresh token string
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BearerToken() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BearerToken"/> class using the passed parameters
        /// </summary>
        public BearerToken(string jwtToken, string refreshToken)
            => (Token, RefreshToken) = (jwtToken, refreshToken);
    }
}
