using FluentValidation;
using IMS.Application.Common.Validators;

namespace IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan
{
    internal class CreateInternetPlanCommandValidator : BaseValidator<CreateInternetPlanCommand>
    {
        public CreateInternetPlanCommandValidator()
        {
            RuleForRequiredString(x => x.Name, "Name");

            RuleForRequiredString(x => x.Description, "Description");

            RuleFor(x => x.SpeedMbps)
                .NotEmpty().WithMessage("SpeedMbps is required.")
                .GreaterThan(0).WithMessage("Speed must be at least 1 Mbps.");

            RuleFor(x => x.Price)
               .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
               .PrecisionScale(18, 2, true).WithMessage("Price must have a maximum of 2 decimal places.");
        }
    }
}
