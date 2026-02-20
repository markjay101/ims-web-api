using IMS.Application.Common.Validators;

namespace IMS.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod
{
    internal class UpdatePaymentMethodCommandValidator : BaseValidator<UpdatePaymentMethodCommand>
    {
        public UpdatePaymentMethodCommandValidator()
        {
            RuleForId(v => v.Id);
        }
    }
}
