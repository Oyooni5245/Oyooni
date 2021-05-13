namespace Oyooni.Server.Requests.Accounts
{
    /// <summary>
    /// Represents a login request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="LoginRequest"/> class using the passed parameters
        /// </summary>
        public LoginRequest(string email, string password) => (Email, Password) = (email, password);
    }
}
