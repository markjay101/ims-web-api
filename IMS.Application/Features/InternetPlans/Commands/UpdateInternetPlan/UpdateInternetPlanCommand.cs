using IMS.Application.Common.Interfaces;
using MediatR;

namespace IMS.Application.Features.InternetPlans.Commands.UpdateInternetPlan
{
    public record UpdateInternetPlanCommand(Guid Id, string Name, string Description, int SpeedMbps, decimal Price) : IRequest<bool>;
    public class UpdateInternetPlanCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateInternetPlanCommand, bool>
    {
        public async Task<bool> Handle(UpdateInternetPlanCommand request, CancellationToken cancellationToken)
        {
            var internetPlan = await context.InternetPlans.FindAsync(request.Id, cancellationToken);

            internetPlan?.Name = request.Name;
            internetPlan?.Description = request.Description;
            internetPlan?.SpeedMbps = request.SpeedMbps;
            internetPlan?.Price = request.Price;

            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
