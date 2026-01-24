using FluentValidation;
using IMS.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

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
                .EmailAddress().WithMessage("Email should be a valid Email Address.");

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

        private bool ValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }

        private async Task<bool> InternetPlanShouldExist(string guid, CancellationToken token)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                return false;

            return await _context.InternetPlans.AnyAsync(ip => ip.Id == parsedGuid, token);
        }
    }
}
