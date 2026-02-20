using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Commands.CreateUserAdminOrSuperAdmin
{
    public class CreateUserAdminOrSuperAdminCommandValidator : BaseValidator<CreateUserAdminOrSuperAdminCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserAdminOrSuperAdminCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleForRequiredEmail(v => v.UserName, "UserName")
                .MustAsync(ShouldBeUniquuUserName).WithMessage("UserName is already used.");

            RuleForRequiredString(v => v.FirstName, "FirstName");

            RuleForRequiredString(v => v.LastName, "LastName");

            RuleFor(v => v.Role)
                .Must(ShouldBeSuperAdminOrAdminRole)
                .WithMessage("You can only create users with SuperAdmin or Admin roles.");
        }

        private async Task<bool> ShouldBeUniquuUserName(string userName, CancellationToken cancellationToken)
        {
            return !await _context.Users.AnyAsync(t => t.UserName == userName, cancellationToken);
        }

        private bool ShouldBeSuperAdminOrAdminRole(Role role)
        {
            return role == Role.SuperAdmin || role == Role.Admin;
        }
    }
}
