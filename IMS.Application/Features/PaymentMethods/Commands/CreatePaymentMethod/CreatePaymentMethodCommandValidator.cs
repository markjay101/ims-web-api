using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.PaymentMethods.Commands.CreatePaymentMethod
{
    internal class CreatePaymentMethodCommandValidator : AbstractValidator<CreatePaymentMethodCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentMethodCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(x => x.MethodName)
                .NotEmpty().WithMessage("The MethodName field is required.")
                .IsEnumName(typeof(PaymentMethodEnum), caseSensitive: false).WithMessage("MethodName must be a valid payment method.")
                .MustAsync(ShouldNotExist).WithMessage(m => $"Payment method ${m.MethodName} already exist.");

            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("The AccountName field is required.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("The AccountNumber field is required.");
        }

        private async Task<bool> ShouldNotExist(string methodName, CancellationToken token)
        {
            return !await _context.PaymentMethods
                .AnyAsync(pm => pm.MethodName.ToString().Equals(methodName, StringComparison.OrdinalIgnoreCase), token);
        }
    }
}
