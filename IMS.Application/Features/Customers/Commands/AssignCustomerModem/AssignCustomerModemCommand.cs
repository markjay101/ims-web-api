using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Customers.Queries;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using IMS.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Customers.Commands.AssignCustomerModem
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record AssignCustomerModemCommand(string CustomerId, string ModemId) : IRequest<CustomerDto?>;
    public class AssignCustomerModemCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<AssignCustomerModemCommand, CustomerDto?>
    {
        public async Task<CustomerDto?> Handle(AssignCustomerModemCommand request, CancellationToken cancellationToken)
        {
            var customerGuid = Guid.Parse(request.CustomerId);
            var customer = await context.Customers.Include(c => c.Plan)
                                                .Include(c => c.Application)
                                                .FirstOrDefaultAsync(c => c.Id == customerGuid);

            if(customer != null)
            {
                customer.Status = CustomerStatus.Active;
                customer.ModemId = Guid.Parse(request.ModemId);

                if(customer.Plan != null)
                {
                    var dateNow = DateTime.UtcNow;
                    customer.Plan.StartDate = dateNow;
                    customer.Plan.NextDueDate = dateNow.AddMonths(1);
                }

                customer.AddDomainEvent(new AssignedCustomerModemEvent(customer));

                await context.SaveChangesAsync();

                return mapper.Map<CustomerDto>(customer);
            }

            return null;

        }
    }
}
