using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using MediatR;

namespace IMS.Application.Features.Modems.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetModemsQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<ModemDto>>;
    public class GetModemsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetModemsQuery, PaginatedList<ModemDto>>
    {
        public async Task<PaginatedList<ModemDto>> Handle(GetModemsQuery request, CancellationToken cancellationToken)
        {
            return await context.Modems
                                .ProjectTo<ModemDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
