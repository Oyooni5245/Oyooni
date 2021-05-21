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
            public const string ProfileUpdateSuccess = "Profile has been updated successully";
            public const string ProfileRetrieved = "Profile has been retrieved successully";
            public const string IncorrectPassword = "Incorrect password";

        }

        public static class AvailableTimes
        {
            public const string SameTimeExists = "Time already exists";
            public const string TimeAlreadyCovered = "Time is already containd in your available times";
            public const string TimeNotFound = "The available time does not exist";
            public const string TimeDeleted = "The available time has been deleted successully";
            public const string AvailableTimesRetrieved = "Available times have been retrieved successfully";
            public const string TimeAdded = "Available time has been added successfully";
        }

        public static class AI
        {
            public const string DigitRecognitionSuccess = "Digit has been recognized";
            public const string ColorRecognitionSuccess = "Colors have been recognized";
        }
    }
}
