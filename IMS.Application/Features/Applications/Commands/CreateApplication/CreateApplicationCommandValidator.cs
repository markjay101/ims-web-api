using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Commands.CreateApplication
{
    public class CreateApplicationCommandValidator : BaseValidator<CreateApplicationCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateApplicationCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleForRequiredString(v => v.FirstName, "FirstName");

            RuleForRequiredString(v => v.LastName, "LastName");

            RuleForRequiredEmail(v => v.Email)
                .MustAsync(EmailShouldBeUnique).WithMessage("Email Address is already used.");

            RuleFor(v => v.ContactNumber)
               .NotEmpty().WithMessage("ContactNumber is required.")
               .Matches(@"^\d+$").WithMessage("ContactNumber must only contain numbers.");

            RuleForRequiredString(v => v.Address, "Address");

            RuleForRequiredString(v => v.City, "City");

            RuleForRequiredString(v => v.Country, "Country");

            RuleForRequiredString(v => v.PostalCode, "PostalCode");

            RuleForId(v => v.InternetPlanId, "InternetPlanId")
                .MustAsync(InternetPlanShouldExist).WithMessage("The Internet Plan does not exist.");
        }

        private async Task<bool> EmailShouldBeUnique(string email, CancellationToken token)
        {
            var e = email.ToLower();

            if (await _context.Applications.AnyAsync(a => a.Email.ToLower() == e, token))
                return false;

            if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == e, token))
                return false;

            return true;
        }

        private async Task<bool> InternetPlanShouldExist(Guid id, CancellationToken token)
        {
            return await _context.InternetPlans.AnyAsync(ip => ip.Id == id, token);
        }
    }
}
