using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Requests.Accounts;
using Oyooni.Server.Extensions;

namespace Oyooni.Server.Validators.Accounts
{
    public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
    {
        public EditProfileRequestValidator()
        {
            RuleFor(req => req.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.FirstNameRequired)
                .Length(3, 32).WithMessage(ValidationResponses.Accounts.FirstNameLength);

            RuleFor(req => req.LastName)
                .Length(3, 64).WithMessage(ValidationResponses.Accounts.LastNameLength)
                .When(req => !req.LastName.IsNullOrEmptyOrWhiteSpaceSafe());
        }
    }
}
