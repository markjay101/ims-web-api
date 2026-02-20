using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.InternetPlans.Queries;
using MediatR;

namespace IMS.Application.Features.InternetPlans.Commands.UpdateInternetPlan
{
    public record UpdateInternetPlanCommand(Guid Id, string Name, string Description, int SpeedMbps, decimal Price) : IRequest<InternetPlanDto?>;
    public class UpdateInternetPlanCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateInternetPlanCommand, InternetPlanDto?>
    {
        public async Task<InternetPlanDto?> Handle(UpdateInternetPlanCommand request, CancellationToken cancellationToken)
        {
            var internetPlan = await context.InternetPlans.FindAsync(request.Id, cancellationToken);

            if(internetPlan == null)
                return null;

            internetPlan.Name = request.Name;
            internetPlan.Description = request.Description;
            internetPlan.SpeedMbps = request.SpeedMbps;
            internetPlan.Price = request.Price;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<InternetPlanDto>(internetPlan);
        }
    }
}
