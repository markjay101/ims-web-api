using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using MediatR;
namespace IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus
{
    public record UpdateInvoiceStatusCommand(Guid InvoiceId, InvoiceStatus Status) : IRequest<bool>;
    public class UpdateInvoiceStatusCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateInvoiceStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            var invoice = await context.Invoices.FindAsync(request.InvoiceId, cancellationToken);
           
            invoice?.Status = request.Status;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
