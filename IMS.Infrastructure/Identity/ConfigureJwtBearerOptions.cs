using IMS.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IMS.Infrastructure.Identity
{
    public class ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions) : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtSettings = jwtOptions.Value;
        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
            }; ;
        }
    }
}
