using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IMS.Application.Features.Auth.Commands.SignIn
{
    public record SignInCommand(string UserName, string Password) : IRequest<UserTokenDto?>;

    public class SignInCommandHandler(IIdentityService identityService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : IRequestHandler<SignInCommand, UserTokenDto?>
    {
        public async Task<UserTokenDto?> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var (token, user) = await identityService.AuthenticateAsync(request.UserName, request.Password);

            if (user is null) return null;

            var ipAddress = identityService.GetIpAddress();

            await identityService.SetRefreshTokenCookie(user.Id, ipAddress, httpContextAccessor.HttpContext!.Response);

            return new UserTokenDto
            {
                Token = token,
                User = mapper.Map<UserDto>(user),
            };
        }
    }
}
