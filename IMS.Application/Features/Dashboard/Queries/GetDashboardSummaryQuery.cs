using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;

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
                return await context.GetSuperAdminDashboardSummary(cancellationToken);

            else if (userRole == UserRoles.Admin)
                return await context.GetAdminDashboardSummary(cancellationToken);

            return null;
        }
    }
}
