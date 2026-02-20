using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Application.Features.Users.Queries;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Users.Commands.UpdateUserAdminOrSuperAdmin
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record UpdateUserAdminOrSuperAdminCommand(Guid Id, string FirstName, string LastName, Role Role) : IRequest<UserDto?>;
    public class UpdateUserAdminOrSuperAdminCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateUserAdminOrSuperAdminCommand, UserDto?>
    {
        public async Task<UserDto?> Handle(UpdateUserAdminOrSuperAdminCommand request, CancellationToken cancellationToken)
        {
            var adminUser = await context.Users.FindAsync(request.Id, cancellationToken);

            if (adminUser == null) return null;

            adminUser.FirstName = request.FirstName;
            adminUser.LastName = request.LastName;
            adminUser.Role = request.Role;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<UserDto>(adminUser);
        }
    }
}
