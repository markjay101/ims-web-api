using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace IMS.Application.Features.Customers.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetCustomersQuery(int PageNumber = 1, int PageSize = 25, string? SearchTerm = null, CustomerStatus? Status = null) : IRequest<CustomerListWithStatusCounts>;
    public class GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCustomersQuery, CustomerListWithStatusCounts>
    {
        public async Task<CustomerListWithStatusCounts> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Customers.AsQueryable();

            var statusCounts = await query.GroupBy(c => c.Status)
                                        .Select(g => new { Status = g.Key, Count = g.Count() })
                                        .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);

            var pendingTotalCount = statusCounts.GetValueOrDefault(CustomerStatus.Pending, 0);
            var activeTotalCount = statusCounts.GetValueOrDefault(CustomerStatus.Active, 0);
            var inactiveTotalCount = statusCounts.GetValueOrDefault(CustomerStatus.Inactive, 0);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(a => a.Application.FirstName.Contains(request.SearchTerm)
                                        || a.Application.LastName.Contains(request.SearchTerm)
                                        || a.Email.Contains(request.SearchTerm));
            }

            if (request.Status.HasValue)
                query = query.Where(a => a.Status == request.Status.Value);

            var paginatedListResult =  await query.ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new CustomerListWithStatusCounts(paginatedListResult, request.PageSize)
            {
                PendingTotalCount = pendingTotalCount,
                ActiveTotalCount = activeTotalCount,
                InactiveTotalCount = inactiveTotalCount
            };
        }
    }
}
