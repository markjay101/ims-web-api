using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.PaymentMethods.Queries;
using MediatR;

namespace IMS.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record UpdatePaymentMethodCommand(Guid Id, string AccountName, string AccountNumber) : IRequest<PaymentMethodDto?>;
    public class UpdatePaymentMethodCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethodDto?>
    {
        public async Task<PaymentMethodDto?> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            var paymentMethod = await context.PaymentMethods.FindAsync(request.Id, cancellationToken);

            if (paymentMethod == null) return null;
            
            paymentMethod.AccountName = request.AccountName;
            paymentMethod.AccountNumber = request.AccountNumber;

            var result = await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<PaymentMethodDto>(paymentMethod);
        }
    }
}
