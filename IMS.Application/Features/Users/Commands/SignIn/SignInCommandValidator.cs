using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        private readonly IApplicationDbContext _context;
        public SignInCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .EmailAddress().WithMessage("UserName should be a valid Email Address.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\!\?\*\.]").WithMessage("Password must contain at least one special character (!?*.).");
        }
    }
}
