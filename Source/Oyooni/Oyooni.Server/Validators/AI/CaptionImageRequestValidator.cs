using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Extensions;
using Oyooni.Server.Requests.AI;
using System;

namespace Oyooni.Server.Validators.AI
{
    /// <summary>
    /// Represents a validator for the <see cref="CaptionImageRequest"/> type
    /// </summary>
    public class CaptionImageRequestValidator : AbstractValidator<CaptionImageRequest>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CaptionImageRequestValidator()
        {
            // Make the value of LangaugeId one of the values of the CaptioningLangauge enum values
            RuleFor(req => req.LanguageId)
                .Must(v => Enum.IsDefined(typeof(CaptioningLanguage), v)).WithMessage(ValidationResponses.AI.InvalidLanguageId);

            // Image file validation
            RuleFor(req => req.File)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(ValidationResponses.General.ImageRequired)
                .Must(file => file.FileName.IsImageFileName()).WithMessage(ValidationResponses.General.InvalidImageExtension)
                .Must(file => file.Length <= Sizes.MaxFileSize).WithMessage(ValidationResponses.General.LargeImageSize);
        }
    }
}
