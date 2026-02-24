using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IMS.Application.Features.Users.Commands.SignIn
{
    public record SignInCommand(string UserName, string Password) : IRequest<UserTokenDto?>;

    public class SignInCommandHandler(IIdentityService identityService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : IRequestHandler<SignInCommand, UserTokenDto?>
    {
        public async Task<UserTokenDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.AuthenticateAsync(request.UserName, request.Password);

            if (user is null) return null;

            var ipAddress = identityService.GetIpAddress();

            await identityService.SetRefreshTokenCookie(user, ipAddress, httpContextAccessor.HttpContext!.Response);

            return new UserTokenDto
            {
                Token = user.Accesstoken,
                User = mapper.Map<UserDto>(user),
            };
        }
    }
}
