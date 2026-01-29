namespace IMS.Application.Features.Users.Queries
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
