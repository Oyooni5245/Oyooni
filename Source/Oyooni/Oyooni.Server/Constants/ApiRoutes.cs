namespace Oyooni.Server.Constants
{
    /// <summary>
    /// The routes of the endpoints in the application server
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// Routes of the accounts
        /// </summary>
        public static class Accounts
        {
            public const string Login = "/api/accounts/login";
            public const string RefreshToken = "/api/accounts/refresh-token";
            public const string Signup = "/api/accounts/signup";
            public const string Profile = "/api/accounts/profile";
            public const string ChangePassword = "/api/accounts/change-password";
        }

        public static class AvailableTimes
        {
            public const string Base = "/api/availabletimes";
            public const string DeleteAvailbleTime = "/api/availabletimes/{id}";
        }

        public static class AI
        {
            public const string RecognizeDigit = "/api/ai/recognize-digit";
            public const string RecognizeColor = "/api/ai/recognize-color";
            public const string CaptionImage = "/api/ai/caption-image";
            public const string EnglishVQA = "/api/ai/visually-answer";
        }
    }
}
