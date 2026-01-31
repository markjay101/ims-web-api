using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Commands.UpdateUserAdminOrSuperAdmin
{
    internal class UpdateUserAdminOrSuperAdminCommandValidator : AbstractValidator<UpdateUserAdminOrSuperAdminCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserAdminOrSuperAdminCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Id)
              .NotEmpty().WithMessage("The Id field is required.")
              .MustAsync(AdminShouldExist).WithMessage("Admin not exist.");

            RuleFor(v => v.FirstName)
               .NotEmpty().WithMessage("The FirstName field is required.");

            RuleFor(v => v.LastName)
              .NotEmpty().WithMessage("The LastName field is required.");

            RuleFor(v => v.Role)
                .Must(BeSuperAdminOrAdminRole).WithMessage("You can only update users with SuperAdmin or Admin roles.");
        }

        private bool BeSuperAdminOrAdminRole(Role role)
        {
            if(role == Role.SuperAdmin || role == Role.Admin)
                return true;

            return false;
        }

        private async Task<bool> AdminShouldExist(Guid guid, CancellationToken token)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == guid && (u.Role == Role.Admin || u.Role == Role.SuperAdmin), token);
        }
    }
}
