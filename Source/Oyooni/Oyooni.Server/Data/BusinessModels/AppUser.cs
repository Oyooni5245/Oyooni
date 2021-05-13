using Microsoft.AspNetCore.Identity;
using Oyooni.Server.Extensions;
using System.Collections.Generic;

namespace Oyooni.Server.Data.BusinessModels
{
    /// <summary>
    /// Represents an application user
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUser()
        {
            // Initialize the refresh tokens collection
            RefreshTokens = new HashSet<RefreshToken>();
        }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's full name (a concatenation of the <see cref="FirstName"/> and the <see cref="LastName"/>)
        /// </summary>
        public string FullName => $"{FirstName}{(LastName.IsNullOrEmptyOrWhiteSpaceSafe() ? string.Empty : " " + LastName)}";
        
        /// <summary>
        /// The refresh tokens related to the user
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
