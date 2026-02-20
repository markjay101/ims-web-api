using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public class SignInCommandValidator : BaseValidator<SignInCommand>
    {
        public SignInCommandValidator(IApplicationDbContext context)
        {
            RuleForRequiredEmail(v => v.UserName, "UserName");

            RuleForRequiredString(v => v.Password, "Password")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\!\?\*\.]").WithMessage("Password must contain at least one special character (!?*.).");
        }
    }
}
