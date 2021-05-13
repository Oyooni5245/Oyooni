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
        }
    }
}
