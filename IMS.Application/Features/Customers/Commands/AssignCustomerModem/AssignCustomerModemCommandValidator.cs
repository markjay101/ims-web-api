using FluentValidation;

namespace IMS.Application.Features.Customers.Commands.AssignCustomerModem
{
    internal class AssignCustomerModemCommandValidator : AbstractValidator<AssignCustomerModemCommand>
    {
        public AssignCustomerModemCommandValidator()
        {
            RuleFor(v => v.CustomerId)
                .Must(ShouldBeValidId).WithMessage("CustomerId is not valid Id.");

            RuleFor(v => v.ModemId)
                .Must(ShouldBeValidId).WithMessage("CustomerId is not valid Id.");
        }

        private bool ShouldBeValidId(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
