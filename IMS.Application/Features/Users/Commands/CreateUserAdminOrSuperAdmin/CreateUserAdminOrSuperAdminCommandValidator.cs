using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Commands.CreateUserAdminOrSuperAdmin
{
    public class CreateUserAdminOrSuperAdminCommandValidator : AbstractValidator<CreateUserAdminOrSuperAdminCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserAdminOrSuperAdminCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.UserName)
                .NotEmpty().WithMessage("The UserName field is required.")
                .EmailAddress().WithMessage("UserName should be a valid Email Address.")
                .MustAsync(BeUniqueUserName).WithMessage("This Email Address is already registered.");

            RuleFor(v => v.FirstName)
               .NotEmpty().WithMessage("The FirstName field is required.");

            RuleFor(v => v.LastName)
               .NotEmpty().WithMessage("The LastName field is required.");

            RuleFor(v => v.Role)
                .Must(BeSuperAdminOrAdminRole)
                .WithMessage("You can only create users with SuperAdmin or Admin roles.");
        }

        private async Task<bool> BeUniqueUserName(string userName, CancellationToken cancellationToken)
        {
            return !await _context.Users.AnyAsync(t => t.UserName == userName, cancellationToken);
        }

        private bool BeSuperAdminOrAdminRole(Role role)
        {
            return role == Role.SuperAdmin || role == Role.Admin;
        }
    }
}
