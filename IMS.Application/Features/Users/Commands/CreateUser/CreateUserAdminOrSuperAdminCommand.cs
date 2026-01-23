using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IMS.Application.Features.Users.Commands.CreateUser
{
    [Authorize(Role = "SuperAdmin")]
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
            var defaultPassword = $"{newAdmin.LastName[..2]}@{shortGuid}";

            var result = await userManager.CreateAsync(newAdmin, defaultPassword);

            return result.Succeeded;
        }
    }
}
