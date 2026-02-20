using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Payments.Commands.CreatePayment
{
    internal class CreatePaymentCommandValidator : BaseValidator<CreatePaymentCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleForId(x => x.InvoiceId, "InvoiceId")
                .MustAsync(InvoiceShouldExist).WithMessage("Invoice does not exist.");

            RuleForEnum(x => x.PaymentMethod, "PaymentMethod");
        }

        private async Task<bool> InvoiceShouldExist(Guid id, CancellationToken token)
        {
            return await _context.Invoices.AnyAsync(i => i.Id == id, token);
        }
    }
}
