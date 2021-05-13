namespace Oyooni.Server.Dtos.Accounts
{
    /// <summary>
    /// Represents an auth token contract
    /// </summary>
    public interface IAuthToken
    {
        /// <summary>
        /// The token string
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// The refresh token string
        /// </summary>
        string RefreshToken { get; set; }
    }
}
