
using IMS.Application.Features.Users.Commands.SignIn;

namespace IMS.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<UserTokenDto?> AuthenticateAsync(string username, string password);
    }
}
