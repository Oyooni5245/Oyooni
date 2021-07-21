using System;

namespace Oyooni.Server.Options
{
    /// <summary>
    /// Represents the authentication options 
    /// </summary>
    public class AuthTokenOptions
    {
        /// <summary>
        /// Issuer of the auth tokens
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// Audience of the auth tokens
        /// </summary>
        public string Audience { get; }

        /// <summary>
        /// Key used for Hashing
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Token expiry timespan
        /// </summary>
        public TimeSpan JwtExpiryTime { get; }

        /// <summary>
        /// Refresh token expiry timespan
        /// </summary>
        public TimeSpan RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="AuthTokenOptions"/> class using the passed parameters
        /// </summary>
        public AuthTokenOptions
            (string issuer, string audience, string key, string jwtExpiryTime, string refreshTokenExpiryTime)
            => (Issuer, Audience, Key, JwtExpiryTime, RefreshTokenExpiryTime)
            = (issuer, audience, key, TimeSpan.Parse(jwtExpiryTime), TimeSpan.Parse(refreshTokenExpiryTime));
    }
}
