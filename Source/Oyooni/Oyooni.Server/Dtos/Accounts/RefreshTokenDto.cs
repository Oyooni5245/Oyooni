namespace Oyooni.Server.Dtos.Accounts
{
    /// <summary>
    /// Represents a refreh token data transfer object
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// The new token
        /// </summary>
        public string NewToken { get; set; }

        /// <summary>
        /// The new refresh token
        /// </summary>
        public string NewRefreshToken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RefreshTokenDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="RefreshTokenDto"/> class using the passed parameters
        /// </summary>
        /// <param name="newToken"></param>
        /// <param name="newRefreshToken"></param>
        public RefreshTokenDto(string newToken, string newRefreshToken)
            => (NewToken, NewRefreshToken) = (newToken, newRefreshToken);
    }
}
