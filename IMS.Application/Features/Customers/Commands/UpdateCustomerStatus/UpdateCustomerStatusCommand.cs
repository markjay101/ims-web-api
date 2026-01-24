using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Customers.Commands.UpdateCustomerStatus
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record UpdateCustomerStatusCommand(Guid Id, CustomerStatus Status) : IRequest<bool>;
    public class UpdateCustomerStatusCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateCustomerStatusCommand, bool>
    {
        public async Task<bool> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Customers.FindAsync(request.Id, cancellationToken);

            customer?.Status = request.Status;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result != 0;
        }
    }
}
