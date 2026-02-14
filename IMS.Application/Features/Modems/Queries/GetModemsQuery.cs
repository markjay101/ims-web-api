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
    public record GetModemsQuery(int PageNumber = 1, int PageSize = 25, string? SearchTerm = null) : IRequest<PaginatedList<ModemDto>>;
    public class GetModemsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetModemsQuery, PaginatedList<ModemDto>>
    {
        public async Task<PaginatedList<ModemDto>> Handle(GetModemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Modems.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(m => m.Model.Contains(request.SearchTerm)
                                        || m.SerialNumber.Contains(request.SearchTerm));

            return await query.ProjectTo<ModemDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
