using IMS.Application.Common.Interfaces;
using MediatR;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public record SignInCommand(string UserName, string Password) : IRequest<string?>;

    public class SignInCommandHandler(IIdentityService identityService) : IRequestHandler<SignInCommand, string?>
    {
        public async Task<string?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return await identityService.AuthenticateAsync(request.UserName, request.Password);
        }
    }
}
