using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using IMS.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Application.Features.Users.Commands.CreateUserAdminOrSuperAdmin
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record CreateUserAdminOrSuperAdminCommand(string UserName, string FirstName, string LastName, Role Role) : IRequest<bool>;

    public class CreateUserAdminOrSuperAdminCommandHandler(UserManager<User> userManager) : IRequestHandler<CreateUserAdminOrSuperAdminCommand, bool>
    {
        public async Task<bool> Handle(CreateUserAdminOrSuperAdminCommand request, CancellationToken cancellationToken)
        {
            var newAdmin = new User
            {
                Role = request.Role,
                UserName = request.UserName,
                Email = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,
            };

            var shortGuid = Guid.NewGuid().ToString("N")[..6];
            var namePart = new string([.. request.LastName.Take(2)]);
            var defaultPassword = $"{char.ToUpper(namePart[0])}{namePart[1..]}@{shortGuid}!";

            newAdmin.Password = defaultPassword;
            newAdmin.AddDomainEvent(new CreatedUserEvent(newAdmin));

            var result = await userManager.CreateAsync(newAdmin, defaultPassword);

            return result.Succeeded;
        }
    }
}
