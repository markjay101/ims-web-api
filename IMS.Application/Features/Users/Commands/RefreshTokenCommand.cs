using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Users.Commands
{
    public record RefreshTokenCommand : IRequest<string?>;
    public class RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, string?>
    {
        public async Task<string?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var token = httpContext?.Request.Cookies["refreshToken"];;

            var storedToken = await context.RefreshTokens
                                            .Include(rt => rt.User)
                                            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);


            if (storedToken == null || !storedToken.IsActive || storedToken.User == null) return null;

            storedToken.Revoked = DateTime.UtcNow;

            var newAccessToken = identityService.GenerateJwtToken(storedToken.User);
            var ipAddress = identityService.GetIpAddress();

            await identityService.SetRefreshTokenCookie(storedToken.User.Id, ipAddress, httpContext!.Response);

            return newAccessToken;
        }
    }
}
