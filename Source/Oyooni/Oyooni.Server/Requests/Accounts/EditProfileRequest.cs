namespace Oyooni.Server.Requests.Accounts
{
    /// <summary>
    /// Represents an edit profile request
    /// </summary>
    public class EditProfileRequest
    {
        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditProfileRequest() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="EditProfileRequest"/> class using the passed parameters
        /// </summary>s
        public EditProfileRequest(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);
    }
}
