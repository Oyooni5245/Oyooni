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
            public const string ServiceUnavailable = "Service is unavailable";
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
            public const string BankNoteDetectionSuccess = "Bank note has been recognized";
            public const string ColorRecognitionSuccess = "Colors have been recognized";
            public const string ImageCaptionedSuccess = "Image has been captioned successfully";
        }

        public static class Hub
        {
            public const string NotAVisualImpaired = "You can't request help from a volunteer";
            public const string AlreadyInACall = "You are already in a call";
            public const string AlreadyGotAcceptedForACall = "Visually impaired already got accepted for a call by another volunteer";
            public const string AlreadyRequestedForHelp = "You already requested for help";
            public const string VolunteerDisconnected = "Volunteer has disconnected";
            public const string VisuallyImpairedDisconnected = "The visually impaired person has disconnected";
            public const string CantAcceptCall = "Can't accept the call";
            public const string VICancelledHelpRequest = "The visually impaired person has cancelled the help request";
        }
    }
}
