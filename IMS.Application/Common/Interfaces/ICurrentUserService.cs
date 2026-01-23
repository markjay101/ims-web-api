namespace IMS.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? FirstName { get; }
        string? LastName { get; }
        string? FullName { get; }
        string? Email { get; }
        string? Role { get; }
    }
}