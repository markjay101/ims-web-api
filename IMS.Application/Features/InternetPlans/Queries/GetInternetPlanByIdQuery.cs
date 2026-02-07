using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.InternetPlans.Queries
{
    [Authorize]
    public record GetInternetPlanByIdQuery(Guid Id) : IRequest<InternetPlanDto?>;
    public class GetInternetPlanByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetInternetPlanByIdQuery, InternetPlanDto?>
    {
        public async Task<InternetPlanDto?> Handle(GetInternetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.InternetPlans.AsNoTracking()
                                        .Where(ip => ip.Id == request.Id)
                                        .ProjectTo<InternetPlanDto>(mapper.ConfigurationProvider)
                                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
