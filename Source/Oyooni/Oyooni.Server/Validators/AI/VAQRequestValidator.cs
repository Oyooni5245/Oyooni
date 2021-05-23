using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AI;

namespace Oyooni.Server.Validators.AI
{
    /// <summary>
    /// Represents a validator for the <see cref="VQARequest"/> type
    /// </summary>
    public class VQARequestValidator : AbstractValidator<VQARequest>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public VQARequestValidator()
        {
            // Make the question required and not too long(256 characters max)
            RuleFor(req => req.Question)
                .Must(value => !value.IsNullOrEmptyOrWhiteSpaceSafe()).WithMessage(ValidationResponses.AI.QuestionRequired)
                .MaximumLength(256).WithMessage(ValidationResponses.AI.QuestionTooLong);

            // Image file validation
            RuleFor(req => req.File)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(ValidationResponses.General.ImageRequired)
                .Must(file => file.FileName.IsImageFileName()).WithMessage(ValidationResponses.General.InvalidImageExtension)
                .Must(file => file.Length <= Sizes.MaxFileSize).WithMessage(ValidationResponses.General.LargeImageSize);
        }
    }
}
