using FluentValidation;

namespace IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan
{
    internal class CreateInternetPlanCommandValidator : AbstractValidator<CreateInternetPlanCommand>
    {
        public CreateInternetPlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required.");

            RuleFor(x => x.SpeedMbps)
                .NotEmpty().WithMessage("The SpeedMbps field is required.");

            RuleFor(x => x.Price)
               .NotEmpty().WithMessage("The Price field is required.");
        }
    }
}
