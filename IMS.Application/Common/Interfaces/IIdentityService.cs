
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace IMS.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        string? GenerateJwtToken(User user);
        string GetIpAddress();
        Task SetRefreshTokenCookie(User user, string ipAddress, HttpResponse response);
    }
}
