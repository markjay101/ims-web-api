using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Customers.Commands.UpdateCustomerStatus
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record UpdateCustomerStatusCommand(Guid Id, CustomerStatus Status) : IRequest<bool>;
    public class UpdateCustomerStatusCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateCustomerStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Customers
                                        .Include(c => c.Plan)
                                        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer is not null 
                && customer.Plan is null
                && customer.Status == CustomerStatus.Pending 
                && request.Status == CustomerStatus.Active)
            {
                var customerApplication = await context.Applications
                                                        .AsNoTracking()
                                                        .FirstAsync(a => a.Id == customer.ApplicationId, cancellationToken);

                var customerPlan = new CustomerPlan
                {
                    CustomerId = customer.Id,
                    InternetPlanId = customerApplication.InternetPlanId,
                    StartDate = DateTime.UtcNow,
                    NextDueDate = DateTime.UtcNow.AddMonths(1)
                };

                await context.CustomerPlans.AddAsync(customerPlan, cancellationToken);
            }

            customer?.Status = request.Status;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result != 0;
        }
    }
}
