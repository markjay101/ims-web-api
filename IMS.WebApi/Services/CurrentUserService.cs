using IMS.Application.Common.Interfaces;
using System.Security.Claims;

namespace IMS.WebApi.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public string? FirstName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
        public string? LastName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);
        public string? Role => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
