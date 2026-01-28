using IMS.Application.Features.Users.Queries;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public class UserTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }
}
