using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.Accounts;
using System.Linq;

namespace Oyooni.Server.Validators.Accounts
{
    /// <summary>
    /// Represents a validator for the <see cref="SignupRequest"/> class
    /// </summary>
    public class SignupRequestValidator : AbstractValidator<SignupRequest>
    {
        public SignupRequestValidator()
        {
            // Email must be not empty and a valid email address
            RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.EmailRequired)
                .EmailAddress().WithMessage(ValidationResponses.Accounts.InvalidEmail);

            // First name must be not empty and not whitespace and with a length of 3-32 character
            RuleFor(r => r.FirstName)
                .Cascade(CascadeMode.Stop)
                .Must(v => !v.IsNullOrEmptyOrWhiteSpaceSafe()).WithMessage(ValidationResponses.Accounts.FirstNameRequired)
                .Length(3, 32).WithMessage(ValidationResponses.Accounts.FirstNameLength);

            // Last name must be 3-64 characters long when it is not emtpy or white space
            RuleFor(r => r.LastName)
                .Cascade(CascadeMode.Stop)
                .Length(3, 64).WithMessage(ValidationResponses.Accounts.LastNameLength)
                .When(req => !req.LastName.IsNullOrEmptyOrWhiteSpaceSafe());

            // Password must not be empty and should have a digit, uppercase, lowercase, and with a minimum length of 6
            RuleFor(o => o.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.PasswordNotEmtpy)
                .Must(v => v.Any(c => char.IsDigit(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveDigit)
                .Must(v => v.Any(c => char.IsUpper(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveUppercase)
                .Must(v => v.Any(c => char.IsLower(c))).WithMessage(ValidationResponses.Accounts.PasswordMustHaveLowercase)
                .MinimumLength(6).WithMessage(ValidationResponses.Accounts.MinLengthPassword);

            // Confirm password must be equal to the password
            RuleFor(o => o.ConfirmPassword)
                .Equal(o => o.Password).WithMessage(ValidationResponses.Accounts.PasswordsNotMatch);
        }
    }
}
