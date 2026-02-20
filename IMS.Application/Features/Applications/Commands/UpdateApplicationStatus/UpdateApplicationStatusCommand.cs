using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Applications.Queries;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Applications.Commands.UpdateApplicationStatus
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record UpdateApplicationStatusCommand(Guid ApplicationId, ApplicationStatus Status) : IRequest<ApplicationDto>;
    public class UpdateApplicationStatusCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateApplicationStatusCommand, ApplicationDto>
    {
        public async Task<ApplicationDto> Handle(UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var application = await context.Applications.FindAsync(request.ApplicationId, cancellationToken);

            if (application!.Status == request.Status)
                return mapper.Map<ApplicationDto>(application);

            application!.Status = request.Status;

            if(request.Status == ApplicationStatus.Approved)
            {
                var newCustomer = mapper.Map<Customer>(application);

                await context.Customers.AddAsync(newCustomer, cancellationToken);

                var customerPlan = new CustomerPlan
                {
                    CustomerId = newCustomer.Id,
                    InternetPlanId = application.InternetPlanId,
                };

                await context.CustomerPlans.AddAsync(customerPlan, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<ApplicationDto>(application);
        }
    }
}
