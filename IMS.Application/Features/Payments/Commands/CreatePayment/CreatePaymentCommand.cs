using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Payments.Commands.CreatePayment
{
    [Authorize(Role = UserRoles.Customer)]
    public record CreatePaymentCommand(string InvoiceId, PaymentMethodEnum PaymentMethod, string ReferenceNumber, decimal Amount) : IRequest<Guid>;
    public class CreatePaymentCommandHandler(IApplicationDbContext context) : IRequestHandler<CreatePaymentCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var newPayment = new Payment
            {
                InvoiceId = Guid.Parse(request.InvoiceId),
                Method = request.PaymentMethod,
                ReferenceNumber = request.ReferenceNumber,
                PaymentDate = DateTime.UtcNow,
                Amount = request.Amount
            };

            await context.Payments.AddAsync(newPayment, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0 ? newPayment.Id : Guid.Empty;
        }
    }
}
