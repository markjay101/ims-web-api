using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Customers.Queries;
using MediatR;

namespace IMS.Application.Features.Customers.Commands.AssignCustomerModem
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record AssignCustomerModemCommand(string CustomerId, string ModemId) : IRequest<CustomerDto?>;
    public class AssignCustomerModemCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<AssignCustomerModemCommand, CustomerDto?>
    {
        public async Task<CustomerDto?> Handle(AssignCustomerModemCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Customers.FindAsync(Guid.Parse(request.CustomerId));

            if(customer != null)
            {
                customer.ModemId = Guid.Parse(request.ModemId);

                await context.SaveChangesAsync();

                return mapper.Map<CustomerDto>(customer);
            }

            return null;

        }
    }
}
