using Oyooni.Server.Common;

namespace Oyooni.Server.Constants
{
    /// <summary>
    /// The validation responses to be sent with the <see cref="ApiErrorResponse"/> object or used with <see cref="FluentValidation"/>-integrated validation
    /// </summary>
    public static class ValidationResponses
    {
        /// <summary>
        /// Accounts-specific validation responses
        /// </summary>
        public static class Accounts
        {
            public const string FirstNameRequired = "First name is required";
            public const string FirstNameLength = "First Name must be 3-32 characters long";
            public const string LastNameLength = "Last Name must be 3-64 characters long";
            public const string EmailRequired = "Email can't be empty";
            public const string InvalidEmail = "Invalid email address";
            public const string PasswordNotEmtpy = "Password can't be empty";
            public const string PasswordsNotMatch = "Passwords do not match";
            public const string PasswordMustHaveDigit = "Password must contain digit(s)";
            public const string PasswordMustHaveUppercase = "Password must have upper case character(s)";
            public const string PasswordMustHaveLowercase = "Password must have lower case character(s)";
            public const string MinLengthPassword = "Password must be at least 6 characters long";
            public const string PasswordRequired = "Password is required";
            public const string PasswordsAreSame = "Password are already the same";
        }

        /// <summary>
        /// General validation responses
        /// </summary>
        public static class General
        {
            public const string InvalidDayOfWeekId = "Invalid day of week identifier";
            public const string InvalidImageExtension = "That is not an image file";
            public const string ImageRequired = "Image is required";
            public const string LargeImageSize = "Image is too large";
        }

        /// <summary>
        /// AvailableTimes-Specific validation responses
        /// </summary>
        public static class AvailableTimes
        {
            public const string InvalidFrom = "'From' value can't be greater than or equal to the 'To' value";
        }

        public static class AI
        {
        }
    }
}
