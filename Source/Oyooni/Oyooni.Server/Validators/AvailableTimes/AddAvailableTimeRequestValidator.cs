using FluentValidation;
using Oyooni.Server.Constants;
using Oyooni.Server.Requests.AvailableTimes;
using System;

namespace Oyooni.Server.Validators.AvailableTimes
{
    /// <summary>
    /// Represents a validator for the <see cref="AddAvailableTimeRequest"/> request
    /// </summary>
    public class AddAvailableTimeRequestValidator : AbstractValidator<AddAvailableTimeRequest>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AddAvailableTimeRequestValidator()
        {
            // Make the day of week id one of the DayOfWeek enum values
            RuleFor(req => req.DayOfWeekId)
                .Must(v => Enum.IsDefined(typeof(DayOfWeek), v)).WithMessage(ValidationResponses.General.InvalidDayOfWeekId);

            // Make the From property not before or equal to the To property
            RuleFor(req => req.From).LessThan(req => req.To).WithMessage(ValidationResponses.AvailableTimes.InvalidFrom);
        }
    }
}
