using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record CreateInternetPlanCommand(string Name, string Description, int SpeedMbps, decimal Price) : IRequest<Guid>;
    public class CreateInternetPlanCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateInternetPlanCommand, Guid>
    {
        public async Task<Guid> Handle(CreateInternetPlanCommand request, CancellationToken cancellationToken)
        {
            var newInternetPlan = new InternetPlan
            {
                Name = request.Name,
                Description = request.Description,
                SpeedMbps = request.SpeedMbps,
                Price = request.Price,
            };

            await context.InternetPlans.AddAsync(newInternetPlan, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return newInternetPlan.Id;    
        }
    }
}
