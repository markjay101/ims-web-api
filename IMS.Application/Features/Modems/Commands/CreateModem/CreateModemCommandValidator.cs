using IMS.Application.Common.Validators;

namespace IMS.Application.Features.Modems.Commands.CreateModem
{
    internal class CreateModemCommandValidator : BaseValidator<CreateModemCommand>
    {
        public CreateModemCommandValidator()
        {
            RuleForRequiredString(x => x.Model, "Model");

            RuleForRequiredString(x => x.SerialNumber, "SerialNumber");

            RuleForRequiredString(x => x.MacAddress, "MacAddress");
        }
    }
}
