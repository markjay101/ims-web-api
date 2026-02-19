using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetApplicationsQuery(int PageNumber = 1, int PageSize = 25, string? SearchTerm = null, ApplicationStatus? Status = null) : IRequest<ApplicationListWithStatusCounts>;
    public class GetApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApplicationsQuery, ApplicationListWithStatusCounts>
    {
        public async Task<ApplicationListWithStatusCounts> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Applications.AsQueryable();

            var statusCounts = await query.GroupBy(c => c.Status)
                                        .Select(g => new { Status = g.Key, Count = g.Count() })
                                        .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);

            var pendingTotalCount = statusCounts.GetValueOrDefault(ApplicationStatus.Pending, 0);
            var approvedTotalCount = statusCounts.GetValueOrDefault(ApplicationStatus.Approved, 0);
            var rejectedTotalCount = statusCounts.GetValueOrDefault(ApplicationStatus.Rejected, 0);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(a => a.FirstName.Contains(request.SearchTerm)
                                        || a.LastName.Contains(request.SearchTerm)
                                        || a.Email.Contains(request.SearchTerm)) ;
            }

            if (request.Status.HasValue)
                query = query.Where(a => a.Status == request.Status.Value);

            var paginatedResult = await query.ProjectTo<ApplicationDto>(mapper.ConfigurationProvider)
                                            .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new ApplicationListWithStatusCounts(paginatedResult, request.PageSize)
            {
                PendingTotalCount = pendingTotalCount,
                ApprovedTotalCount = approvedTotalCount,
                RejectedTotalCount = rejectedTotalCount,
            };
        }
    }
}
