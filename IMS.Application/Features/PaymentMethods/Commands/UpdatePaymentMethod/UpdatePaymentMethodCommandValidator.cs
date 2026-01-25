using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod
{
    internal class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePaymentMethodCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.PaymentMethodId)
                .NotEmpty().WithMessage("The PaymentMethodId field is required.")
                .MustAsync(MustExist).WithMessage("The Payment Method does not exist.");
        }

        private async Task<bool> MustExist(Guid guid, CancellationToken token)
        {
            return await _context.PaymentMethods.AnyAsync(pm => pm.Id == guid, token);
        }
    }
}
