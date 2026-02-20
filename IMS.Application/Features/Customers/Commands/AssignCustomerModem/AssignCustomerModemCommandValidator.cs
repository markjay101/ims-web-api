using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Customers.Commands.AssignCustomerModem
{
    internal class AssignCustomerModemCommandValidator : BaseValidator<AssignCustomerModemCommand>
    {
        private readonly IApplicationDbContext _context;

        public AssignCustomerModemCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleForId(v => v.CustomerId, "CustomerId");

            RuleForId(v => v.ModemId, "ModemId")
                .MustAsync(ModemShouldExist).WithMessage("Modem does not exist.");
        }

        private async Task<bool> ModemShouldExist(Guid id, CancellationToken token)
        {
            return await _context.Modems.AnyAsync(m => m.Id == id, token);
        }
    }
}
