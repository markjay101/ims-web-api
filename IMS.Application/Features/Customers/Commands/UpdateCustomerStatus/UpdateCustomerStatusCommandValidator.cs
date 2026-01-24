using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Customers.Commands.UpdateCustomerStatus
{
    internal class UpdateCustomerStatusCommandValidator : AbstractValidator<UpdateCustomerStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerStatusCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("The CustomerId field is required.")
                .MustAsync(CustomerExists).WithMessage("Customer with the specified Id does not exist.");
            
        }

        private async Task<bool> CustomerExists(Guid guid, CancellationToken token)
        {
            return await _context.Customers.AnyAsync(c => c.Id == guid, token);
        }
    }
}
