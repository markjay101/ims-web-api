using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Applications.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetApplicationsQuery(int PageNumber = 1, int PageSize = 25, string? SearchTerm = null, ApplicationStatus? Status = null) : IRequest<PaginatedList<ApplicationDto>>;
    public class GetApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApplicationsQuery, PaginatedList<ApplicationDto>>
    {
        public async Task<PaginatedList<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Applications.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(a => a.FirstName.Contains(request.SearchTerm)
                                        || a.LastName.Contains(request.SearchTerm)
                                        || a.Email.Contains(request.SearchTerm)) ;
            }

            if (request.Status.HasValue)
                query = query.Where(a => a.Status == request.Status.Value);

            return await query.ProjectTo<ApplicationDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
