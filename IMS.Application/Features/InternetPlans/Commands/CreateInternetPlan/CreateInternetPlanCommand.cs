using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan
{
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
            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0 ? newInternetPlan.Id : Guid.Empty;    
        }
    }
}
