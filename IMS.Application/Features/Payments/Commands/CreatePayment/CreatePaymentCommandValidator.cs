using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Payments.Commands.CreatePayment
{
    internal class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required.")
                .Must(ValidGuid).WithMessage("InvoiceId is invalid.")
                .MustAsync(InvoiceShouldExist).WithMessage("Invoice with the specified ID does not exist.");
        }

        private async Task<bool> InvoiceShouldExist(string invoiceId, CancellationToken token)
        {
            return await _context.Invoices.AnyAsync(i => i.Id == Guid.Parse(invoiceId), token);
        }

        private bool ValidGuid(string invoiceId)
        {
            return Guid.TryParse(invoiceId, out _);
        }
    }
}
