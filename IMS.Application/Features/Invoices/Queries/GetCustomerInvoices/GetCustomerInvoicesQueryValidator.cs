using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Queries.GetCustomerInvoices
{
    internal class GetCustomerInvoicesQueryValidator : AbstractValidator<GetCustomerInvoicesQuery>
    {
        private readonly IApplicationDbContext _context;

        public GetCustomerInvoicesQueryValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.CustomerId)
                .Must(ValidGuid).WithMessage("Id is not a valid Customer Id.")
                .MustAsync(CustomerExists).WithMessage("Customer does not exist.");
        }

        private async Task<bool> CustomerExists(string customerId, CancellationToken token)
        {
            return await _context.Customers.AnyAsync(c => c.Id.ToString() == customerId, token);
        }

        private bool ValidGuid(string customerId)
        {
            return Guid.TryParse(customerId, out _);  
        }
    }
}
