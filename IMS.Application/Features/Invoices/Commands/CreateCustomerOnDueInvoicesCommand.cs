using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using IMS.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Commands
{
    public record CreateCustomerOnDueInvoicesCommand : IRequest;
    public class CreateCustomerOnDueInvoicesCommandHandler(IApplicationDbContext context, IMediator mediator) : IRequestHandler<CreateCustomerOnDueInvoicesCommand>
    {
        public async Task Handle(CreateCustomerOnDueInvoicesCommand request, CancellationToken cancellationToken)
        {
            var dateToday = DateTime.UtcNow.Date;

            var customersOnDue = await context.Customers
                                            .AsNoTracking()
                                            .Include(c => c.Application)
                                            .Include(c => c.Plan)
                                            .ThenInclude(c => c.InternetPlan)
                                            .AsSplitQuery()
                                            .Where(c => c.Plan != null && c.Plan.NextDueDate <= dateToday)
                                            .ToListAsync(cancellationToken);

            if (customersOnDue.Count == 0) return;

            var newInvoices = new List<Invoice>();
            
            customersOnDue.ForEach(c =>
            {
                newInvoices.Add(new Invoice
                {
                    CustomerId = c.Id,
                    Amount = c.Plan.InternetPlan.Price,
                    DueDate = dateToday,
                    Status = InvoiceStatus.Pending
                });

                c.Plan.NextDueDate = c.Plan.NextDueDate!.Value.AddMonths(1);
            });

            if(newInvoices.Count > 0)
            {
                await context.Invoices.AddRangeAsync(newInvoices, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                await mediator.Publish(new CreatedCustomersOnDueInvoiceEvent(customersOnDue), cancellationToken);
            }
        }
    }
}
