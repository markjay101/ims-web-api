using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Queries.GetCustomerInvoices
{
    internal class GetCustomerInvoicesQueryValidator : BaseValidator<GetCustomerInvoicesQuery>
    {
        private readonly IApplicationDbContext _context;

        public GetCustomerInvoicesQueryValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.CustomerId)
                .MustAsync(CustomerShouldExist).WithMessage("Customer does not exist.");
        }

        private async Task<bool> CustomerShouldExist(Guid customerId, CancellationToken token)
        {
            return await _context.Customers.AnyAsync(c => c.Id == customerId, token);
        }
    }
}
