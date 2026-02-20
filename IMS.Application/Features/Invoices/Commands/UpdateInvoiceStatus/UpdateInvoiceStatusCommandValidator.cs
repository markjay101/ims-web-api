using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus
{
    internal class UpdateInvoiceStatusCommandValidator : BaseValidator<UpdateInvoiceStatusCommand>
    {
        public UpdateInvoiceStatusCommandValidator()
        {
            RuleForId(v => v.Id);
        }
    }
}
