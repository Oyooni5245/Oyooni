using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Requests.Accounts;
using System.Linq;

namespace Oyooni.Server.Validators.Accounts
{
    /// <summary>
    /// Represents a validator for the <see cref="ChangePasswordRequest"/> class
    /// </summary>
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            // Old and new passwords should not be the same
            RuleFor(req => req.OldPassword)
                .NotEqual(req => req.NewPassword).WithMessage(ValidationResponses.Accounts.PasswordsAreSame);

            // New Password must not be empty and should have a digit, uppercase, lowercase, and with a minimum length of 6
            RuleFor(o => o.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.PasswordNotEmtpy)
                .Must(v => v.Any(c => char.IsDigit(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveDigit)
                .Must(v => v.Any(c => char.IsUpper(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveUppercase)
                .Must(v => v.Any(c => char.IsLower(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveLowercase)
                .MinimumLength(6).WithMessage(ValidationResponses.Accounts.MinLengthPassword);

            // Confirm new password must be equal to the password
            RuleFor(o => o.ConfirmNewPassword)
                .Equal(o => o.NewPassword).WithMessage(ValidationResponses.Accounts.PasswordsNotMatch);
        }
    }
}
