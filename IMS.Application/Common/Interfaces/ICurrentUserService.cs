namespace IMS.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? FullName { get; }
        string? Email { get; }
        string? Role { get; }
    }
}