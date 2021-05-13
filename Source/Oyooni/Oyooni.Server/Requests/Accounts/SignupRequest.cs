namespace Oyooni.Server.Requests.Accounts
{
    /// <summary>
    /// Represents a sign up request
    /// </summary>
    public class SignupRequest
    {
        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's email name
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's password name
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The confirm password
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Default consructor
        /// </summary>
        public SignupRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="SignupRequest"/> class using the passed parameters
        /// </summary>
        public SignupRequest(string firstName, string lastName, string email, string password, string confirmPassword)
            => (FirstName, LastName, Email, Password, ConfirmPassword) = (firstName, lastName, email, password, confirmPassword);
    }
}
