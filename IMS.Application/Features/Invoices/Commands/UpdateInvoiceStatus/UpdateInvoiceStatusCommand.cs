using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Invoices.Queries;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using IMS.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record UpdateInvoiceStatusCommand(Guid Id, InvoiceStatus Status) : IRequest<InvoiceDto?>;
    public class UpdateInvoiceStatusCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateInvoiceStatusCommand, InvoiceDto?>
    {
        public async Task<InvoiceDto?> Handle(UpdateInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            var invoice = await context.Invoices.Include(i => i.Payment)
                                                .Include(i => i.Customer)
                                                .Include(i => i.Customer.Application)
                                                .Include(i => i.Customer!.Plan)
                                                .ThenInclude(p => p.InternetPlan)
                                                .AsSplitQuery()
                                                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (invoice == null || (invoice != null && invoice.Status == InvoiceStatus.PaidConfirmed))
                return null;

            if(invoice!.Status == InvoiceStatus.PaidForConfirmation && request.Status == InvoiceStatus.PaidConfirmed)
            {
                invoice.Customer?.Plan?.NextDueDate = invoice.Customer?.Plan?.NextDueDate?.AddMonths(1);

                invoice.AddDomainEvent(new PaidConfirmedEvent(invoice));
            }

            invoice.Status = request.Status;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<InvoiceDto>(invoice);
        }
    }
}
