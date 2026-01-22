
namespace IMS.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> AuthenticateAsync(string username, string password);
    }
}
