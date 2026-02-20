using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Invoices.Queries;
using IMS.Domain.Common.Enums;
using MediatR;
namespace IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record UpdateInvoiceStatusCommand(Guid Id, InvoiceStatus Status) : IRequest<InvoiceDto?>;
    public class UpdateInvoiceStatusCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateInvoiceStatusCommand, InvoiceDto?>
    {
        public async Task<InvoiceDto?> Handle(UpdateInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            var invoice = await context.Invoices.FindAsync(request.Id, cancellationToken);

            if (invoice == null)
                return null;
           
            invoice.Status = request.Status;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<InvoiceDto>(invoice);
        }
    }
}
