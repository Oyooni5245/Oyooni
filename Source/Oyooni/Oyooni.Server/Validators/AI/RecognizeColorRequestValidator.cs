using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AI;

namespace Oyooni.Server.Validators.AI
{
    /// <summary>
    /// Represents a validator for the <see cref="RecognizeColorRequest"/> type
    /// </summary>
    public class RecognizeColorRequestValidator : AbstractValidator<RecognizeColorRequest>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RecognizeColorRequestValidator()
        {
            RuleFor(req => req.File)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(ValidationResponses.General.ImageRequired)
                .Must(file => file.FileName.IsImageFileName()).WithMessage(ValidationResponses.General.InvalidImageExtension)
                .Must(file => file.Length <= Sizes.MaxFileSize).WithMessage(ValidationResponses.General.LargeImageSize);
        }
    }
}
