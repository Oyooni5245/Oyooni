using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Oyooni.Server.Constants;
using Oyooni.Server.Requests.Accounts;

namespace Oyooni.Server.Validators.Accounts
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(req => req.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.EmailRequired)
                .EmailAddress().WithMessage(ValidationResponses.Accounts.InvalidEmail);

            RuleFor(req => req.Password)
                .NotEmpty().WithMessage(ValidationResponses.Accounts.PasswordRequired);
        }
    }
}
