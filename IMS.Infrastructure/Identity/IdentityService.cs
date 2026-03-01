using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using IMS.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IMS.Infrastructure.Identity
{
    internal class IdentityService(
        IApplicationDbContext context, 
        UserManager<User> userManager, 
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> jwtOptions,
        IWebHostEnvironment env) : IIdentityService
    {
        private readonly JwtOptions _jwtSettings = jwtOptions.Value;
        public async Task<(string? Token, User? User)> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                var token = GenerateJwtToken(user);

                return (token, user);
            }

            return (null, null);
        }

        public string? GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SetRefreshTokenCookie(Guid userId, string ipAddress, HttpResponse response)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress,
                User = null,
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            var isDev = env.IsDevelopment();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !isDev,
                SameSite = isDev ? SameSiteMode.Unspecified : SameSiteMode.None,
                Expires = refreshToken.Expires,
            };

            response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        public string GetIpAddress()
        {
            if (httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("X-Forwarded-For") == true)
                return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]!;

            return httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
        }
    }
}
