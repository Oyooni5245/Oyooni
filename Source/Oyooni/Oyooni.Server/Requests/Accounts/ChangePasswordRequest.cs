namespace Oyooni.Server.Requests.Accounts
{
    /// <summary>
    /// Represents a request for changing a user's password
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// The user's old password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// The user's new password
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// The user's confirm new password
        /// </summary>
        public string ConfirmNewPassword { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ChangePasswordRequest"/> class using the passed parameters
        /// </summary>
        public ChangePasswordRequest(string oldPassword, string newPassword, string confirmNewPassword)
            => (OldPassword, NewPassword, ConfirmNewPassword) = (oldPassword, newPassword, confirmNewPassword);
    }
}
