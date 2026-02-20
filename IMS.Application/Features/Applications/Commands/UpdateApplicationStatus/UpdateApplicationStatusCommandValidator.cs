using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandValidator : BaseValidator<UpdateApplicationStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateApplicationStatusCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleForId(v => v.ApplicationId, "ApplicationId")
                .MustAsync(ApplicationShouldExist).WithMessage("Application does not exist.");
        }

        private async Task<bool> ApplicationShouldExist(Guid guid, CancellationToken token)
        {
            return await _context.Applications.AnyAsync(a => a.Id == guid, token);
        }
    }
}
