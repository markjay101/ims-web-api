using IMS.Application.Common.Interfaces;
using MediatR;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public record SignInCommand(string UserName, string Password) : IRequest<UserTokenDto?>;

    public class SignInCommandHandler(IIdentityService identityService) : IRequestHandler<SignInCommand, UserTokenDto?>
    {
        public async Task<UserTokenDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return await identityService.AuthenticateAsync(request.UserName, request.Password);
        }
    }
}
