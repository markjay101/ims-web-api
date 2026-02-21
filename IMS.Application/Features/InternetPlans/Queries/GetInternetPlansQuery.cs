using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using MediatR;

namespace IMS.Application.Features.InternetPlans.Queries
{
    public record GetInternetPlansQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<InternetPlanDto>>;

    public class GetInternetPlansQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetInternetPlansQuery, PaginatedList<InternetPlanDto>>
    {
        public async Task<PaginatedList<InternetPlanDto>> Handle(GetInternetPlansQuery request, CancellationToken cancellationToken)
        {
            return await context.InternetPlans
                                .ProjectTo<InternetPlanDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }

    }
}
