using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.InternetPlans.Queries.GetInternetPlanById
{
    [Authorize]
    public record GetInternetPlanByIdQuery(Guid Id) : IRequest<InternetPlanDto?>;
    public class GetInternetPlanByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetInternetPlanByIdQuery, InternetPlanDto?>
    {
        public async Task<InternetPlanDto?> Handle(GetInternetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var internetPlan = await context.InternetPlans.AsNoTracking().FirstOrDefaultAsync(ip => ip.Id == request.Id, cancellationToken);

            return mapper.Map<InternetPlanDto>(internetPlan);
        }
    }
}
