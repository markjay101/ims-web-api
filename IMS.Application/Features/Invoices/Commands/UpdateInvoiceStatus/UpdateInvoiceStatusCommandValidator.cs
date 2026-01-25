using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus
{
    internal class UpdateInvoiceStatusCommandValidator : AbstractValidator<UpdateInvoiceStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateInvoiceStatusCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.InvoiceId)
                .NotEmpty().WithMessage("The InvoiceId field is required.")
                .MustAsync(InvoiceShouldExist).WithMessage("The Invoice does not exist.");
        }

        private async Task<bool> InvoiceShouldExist(Guid guid, CancellationToken token)
        {
            return await _context.Invoices.AnyAsync(i => i.Id == guid, token);
        }
    }
}
