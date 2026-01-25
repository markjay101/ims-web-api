using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.PaymentMethods.Commands.CreatePaymentMethod
{
    public record CreatePaymentMethodCommand(string MethodName, string AccountName, string AccountNumber) : IRequest<Guid>;
    public class CreatePaymentMethodCommandHandler(IApplicationDbContext context) : IRequestHandler<CreatePaymentMethodCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var newPaymentMethod = new PaymentMethod
            {
                MethodName = Enum.Parse<PaymentMethodEnum>(request.MethodName),
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber
            };

            await context.PaymentMethods.AddAsync(newPaymentMethod, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0 ? newPaymentMethod.Id : Guid.Empty;
        }
    }
}
