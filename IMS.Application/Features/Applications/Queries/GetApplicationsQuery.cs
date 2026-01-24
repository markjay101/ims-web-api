using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using MediatR;

namespace IMS.Application.Features.Applications.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetApplicationsQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<ApplicationDto>>;
    public class GetApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApplicationsQuery, PaginatedList<ApplicationDto>>
    {
        public async Task<PaginatedList<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            return await context.Applications
                                .ProjectTo<ApplicationDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
