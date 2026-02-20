using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.PaymentMethods.Commands.CreatePaymentMethod
{
    internal class CreatePaymentMethodCommandValidator : BaseValidator<CreatePaymentMethodCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentMethodCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleForEnum(x => x.MethodName, "PaymentMethod")
                .MustAsync(ShouldNotExist).WithMessage(m => $"Payment method {m.MethodName} already exist.");

            RuleForRequiredString(x => x.AccountName, "AccountName");

            RuleForRequiredString(x => x.AccountNumber, "AccountNumber");
        }

        private async Task<bool> ShouldNotExist(PaymentMethodEnum @enum, CancellationToken token)
        {
            return !await _context.PaymentMethods
                .AnyAsync(pm => pm.MethodName.ToString().ToLower() == @enum.ToString().ToLower(), token);
        }
    }
}
