using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateApplicationStatusCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.ApplicationId)
                .NotEmpty().WithMessage("The ApplicationId field is required.")
                .MustAsync(ApplicationShouldExist).WithMessage("The Application does not exist.");
        }

        private async Task<bool> ApplicationShouldExist(Guid guid, CancellationToken token)
        {
            return await _context.Applications.AnyAsync(a => a.Id == guid, token);
        }
    }
}
