using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Users.Commands.SignIn;
using IMS.Application.Features.Users.Queries;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IMS.Infrastructure.Identity
{
    public class IdentityService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper) : IIdentityService
    {
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var minutes = configuration["JwtSettings:ExpiryMinutes"];
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(minutes));

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
