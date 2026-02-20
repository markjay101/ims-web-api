using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Customers.Commands.UpdateCustomerStatus
{
    internal class UpdateCustomerStatusCommandValidator : BaseValidator<UpdateCustomerStatusCommand>
    {
        public UpdateCustomerStatusCommandValidator(IApplicationDbContext context)
        {
            RuleForId(v => v.Id);

            RuleForEnum(v => v.Status, "CustomerStatus");
        }
    }
}
