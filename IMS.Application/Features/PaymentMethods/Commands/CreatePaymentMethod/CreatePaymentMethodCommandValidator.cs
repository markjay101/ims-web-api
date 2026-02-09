using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IMS.Application.Features.PaymentMethods.Commands.CreatePaymentMethod
{
    internal class CreatePaymentMethodCommandValidator : AbstractValidator<CreatePaymentMethodCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentMethodCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(x => x.MethodName)
                .MustAsync(ShouldNotExist).WithMessage(m => $"Payment method {m.MethodName} already exist.");

            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("The AccountName field is required.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("The AccountNumber field is required.");
        }

        private async Task<bool> ShouldNotExist(PaymentMethodEnum @enum, CancellationToken token)
        {
            return !await _context.PaymentMethods
                .AnyAsync(pm => pm.MethodName.ToString().ToLower() == @enum.ToString().ToLower(), token);
        }
    }
}
