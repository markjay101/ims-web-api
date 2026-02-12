using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.InternetPlans.Queries.GetInternetPlanById
{
    [Authorize]
    public record GetInternetPlanByIdQuery(string Id) : IRequest<InternetPlanDto?>;
    public class GetInternetPlanByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetInternetPlanByIdQuery, InternetPlanDto?>
    {
        public async Task<InternetPlanDto?> Handle(GetInternetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.Id);

            var internetPlan = await context.InternetPlans.AsNoTracking().FirstOrDefaultAsync(ip => ip.Id == guid, cancellationToken);

            return mapper.Map<InternetPlanDto>(internetPlan);
        }
    }
}
