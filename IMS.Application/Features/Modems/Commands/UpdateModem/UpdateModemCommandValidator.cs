using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Modems.Commands.UpdateModem
{
    public class UpdateModemCommandValidator : AbstractValidator<UpdateModemCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateModemCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The Id field is required.")
                .MustAsync(ModemShouldExist).WithMessage("Modem with the specified Id does not exist.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("The Model field is required.");

            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("The SerialNumber field is required.");

            RuleFor(x => x.MacAddress)
                .NotEmpty().WithMessage("The MacAddress field is required.");
        }

        private async Task<bool> ModemShouldExist(Guid guid, CancellationToken token)
        {
            return await _context.Modems.AnyAsync(m => m.Id == guid, token);
        }
    }
}
