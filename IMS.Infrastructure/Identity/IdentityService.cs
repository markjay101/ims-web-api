using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Users.Commands.SignIn;
using IMS.Application.Features.Users.Queries;
using IMS.Domain.Entities;
using IMS.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IMS.Infrastructure.Identity
{
    public class IdentityService(UserManager<User> userManager, IOptions<JwtOptions> jwtOptions, IMapper mapper) : IIdentityService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        public async Task<UserTokenDto?> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                var token = GenerateJwtToken(user);
                return new UserTokenDto
                {
                    Token = token!,
                    User = mapper.Map<UserDto>(user)
                };
            }

            return null;
        }

        private string? GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
