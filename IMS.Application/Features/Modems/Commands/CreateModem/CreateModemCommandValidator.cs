using FluentValidation;

namespace IMS.Application.Features.Modems.Commands.CreateModem
{
    internal class CreateModemCommandValidator : AbstractValidator<CreateModemCommand>
    {
        public CreateModemCommandValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("The SerialNumber field is required.");

            RuleFor(x => x.MacAddress)
                .NotEmpty().WithMessage("The MacAddress field is required.");
        }
    }
}
