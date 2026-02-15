using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Dashboard.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record GetDashboardSummaryQuery : IRequest<BaseDashboardSummaryDto?>;
    public class GetDashboardSummaryQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService) : IRequestHandler<GetDashboardSummaryQuery, BaseDashboardSummaryDto?>
    {
        public async Task<BaseDashboardSummaryDto?> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var userRole = currentUserService.Role;

            if (userRole == UserRoles.SuperAdmin)
                return await context.SuperAdminDashboardSummary.FromSqlRaw("EXEC sp_GetDashboardSummary").AsNoTracking().AsAsyncEnumerable().FirstAsync(cancellationToken);

            else if (userRole == UserRoles.Admin)
                return await context.AdminDashboardSummary.FromSqlRaw("EXEC sp_GetDashboardSummary").AsNoTracking().AsAsyncEnumerable().FirstAsync(cancellationToken);

            return null;
        }
    }
}
