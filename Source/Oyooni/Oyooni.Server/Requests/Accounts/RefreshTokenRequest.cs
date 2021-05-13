namespace Oyooni.Server.Requests.Accounts
{
    /// <summary>
    /// Represents a refresh token request
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// The token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The refresh token string
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RefreshTokenRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RefreshTokenRequest"/> class using the passed parameters
        /// </summary>
        public RefreshTokenRequest(string jwtToken, string refreshToken)
            => (Token, RefreshToken) = (jwtToken, refreshToken);
    }
}
