using System;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents a refresh token entity
    /// </summary>
    public class RefreshToken : AuditableEntity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RefreshToken()
        {
            // Set the token to a random guid
            Token = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The refresh token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The related jwt identifier
        /// </summary>
        public string Jid { get; set; }

        /// <summary>
        /// Indicates whether the token is used or not
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// Indicates whether the token is invalidated or not
        /// </summary>
        public bool Invalidated { get; set; }

        /// <summary>
        /// The expire date of the token
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}
