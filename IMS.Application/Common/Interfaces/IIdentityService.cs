
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace IMS.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(string? Token, User? User)> AuthenticateAsync(string username, string password);
        string? GenerateJwtToken(User user);
        string GetIpAddress();
        Task SetRefreshTokenCookie(Guid userId, string ipAddress, HttpResponse response);
    }
}
