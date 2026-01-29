using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record GetAdminStatQuery : IRequest<AdminStatDto>;
    public class GetAdminStatQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAdminStatQuery, AdminStatDto>
    {
        public async Task<AdminStatDto> Handle(GetAdminStatQuery request, CancellationToken cancellationToken)
        {
            var stats = await context.Users.Where(u => u.Role == Role.SuperAdmin || u.Role == Role.Admin)
                        .GroupBy(u => u.Role)
                        .Select(g => new
                        {
                            Role = g.Key,
                            Count = g.Count()
                        })
                        .ToListAsync(cancellationToken);

            return new AdminStatDto
            {
                TotalAdmins = stats.FirstOrDefault(x => x.Role == Role.Admin)?.Count ?? 0,
                TotalSuperAdmins = stats.FirstOrDefault(x => x.Role == Role.SuperAdmin)?.Count ?? 0
            };
        }
    }
}
