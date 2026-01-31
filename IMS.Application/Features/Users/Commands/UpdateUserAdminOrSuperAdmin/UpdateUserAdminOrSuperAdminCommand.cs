using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Users.Commands.UpdateUserAdminOrSuperAdmin
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record UpdateUserAdminOrSuperAdminCommand(Guid Id, string FirstName, string LastName, Role Role) : IRequest<bool>;
    public class UpdateUserAdminOrSuperAdminCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateUserAdminOrSuperAdminCommand, bool>
    {
        public async Task<bool> Handle(UpdateUserAdminOrSuperAdminCommand request, CancellationToken cancellationToken)
        {
            var adminUser = await context.Users.FindAsync(request.Id, cancellationToken);

            adminUser?.FirstName = request.FirstName;
            adminUser?.LastName = request.LastName;
            adminUser?.Role = request.Role;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
