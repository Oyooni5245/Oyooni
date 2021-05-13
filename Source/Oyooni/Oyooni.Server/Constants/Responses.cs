using Oyooni.Server.Common;

namespace Oyooni.Server.Constants
{
    /// <summary>
    /// The responses used as message that are sent with the <see cref="ApiResponse"/> object
    /// </summary>
    public static class Responses
    {
        /// <summary>
        /// Set of general responses
        /// </summary>
        public static class General
        {
            public const string UnAuthorizedAction = "Unauthorized action";
            public const string ErrorOccured = "An error occured";
        }

        /// <summary>
        /// Accounts-specific responses
        /// </summary>
        public static class Accounts
        {
            public const string InvalidLogin = "Invalid login attempt";
            public const string EmailNotVerified = "Account is not verified yet";
            public const string LoginSuccess = "Login successfull";
            public const string InvalidRefresh = "Invalid refresh attempt";
            public const string SignupSuccess = "Registration successful";
            public const string AlreadyExist = "Email/Username is taken";
        }
    }
}
