using IMS.Application.Common.Interfaces;
using MediatR;

namespace IMS.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod
{
    public record UpdatePaymentMethodCommand(Guid PaymentMethodId, string AccountName, string AccountNumber) : IRequest<bool>;
    public class UpdatePaymentMethodCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdatePaymentMethodCommand, bool>
    {
        public async Task<bool> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await context.PaymentMethods.FindAsync(request.PaymentMethodId, cancellationToken);
            
            paymentMethod?.AccountName = request.AccountName;
            paymentMethod?.AccountNumber = request.AccountNumber;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
