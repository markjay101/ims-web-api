using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.InternetPlans.Commands.UpdateInternetPlan
{
    internal class UpdateInternetPlanCommandValidator : AbstractValidator<UpdateInternetPlanCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateInternetPlanCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The Id field is required.")
                .MustAsync(InternetPlanShouldExist).WithMessage("Internet plan does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name field is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required.");

            RuleFor(x => x.SpeedMbps)
                .NotEmpty().WithMessage("The SpeedMbps field is required.");
        }

        private async Task<bool> InternetPlanShouldExist(Guid id, CancellationToken token)
        {
            return await _context.InternetPlans.AnyAsync(ip => ip.Id == id, token);
        }
    }
}
