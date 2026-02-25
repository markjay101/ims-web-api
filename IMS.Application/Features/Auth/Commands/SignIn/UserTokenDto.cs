using IMS.Application.Features.Users.Queries;

namespace IMS.Application.Features.Auth.Commands.SignIn
{
    public class UserTokenDto
    {
        public string? Token { get; set; }
        public UserDto User { get; set; } = new();
    }
}
