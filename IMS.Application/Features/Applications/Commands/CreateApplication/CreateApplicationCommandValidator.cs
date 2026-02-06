using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Commands.CreateApplication
{
    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateApplicationCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("The FirstName field is required.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("The LastName field is required.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("The Email field is required.")
                .EmailAddress().WithMessage("Email should be a valid Email Address.")
                .MustAsync(EmailShouldBeUnique).WithMessage("The Email is already use.");

            RuleFor(v => v.ContactNumber)
               .NotEmpty().WithMessage("The ContactNumber field is required.")
               .Matches(@"^\d+$").WithMessage("The ContactNumber must only contain numbers.");

            RuleFor(v => v.Address)
                .NotEmpty().WithMessage("The Address field is required.");

            RuleFor(v => v.City)
                .NotEmpty().WithMessage("The City field is required.");

            RuleFor(v => v.Country)
                .NotEmpty().WithMessage("The Country field is required.");

            RuleFor(v => v.PostalCode)
                .NotEmpty().WithMessage("The PostalCode field is required.");

            RuleFor(v => v.InternetPlanId)
                .NotEmpty().WithMessage("The InternetPlanId field is required.")
                .Must(ValidGuid).WithMessage("The format of the ID is invalid.")
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

        private bool ValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }

        private async Task<bool> InternetPlanShouldExist(string guid, CancellationToken token)
        {
            return await _context.InternetPlans.AnyAsync(ip => ip.Id == Guid.Parse(guid), token);
        }
    }
}
