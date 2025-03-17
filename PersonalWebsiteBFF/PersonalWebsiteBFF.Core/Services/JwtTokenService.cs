using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersonalWebsiteBFF.Core.Helpers;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalWebsiteBFF.Core.Services
{
    public class JwtTokenService(IConfiguration configuration, AppDbContext appDbContext) : IJwtTokenService
    {
        public async Task<string> GenerateToken(User user)
        {
            var issuer = ConfigurationHelper.GetConfigValue(configuration, "AuthSettings:Issuer", "AUTH_ISSUER");
            var audience = ConfigurationHelper.GetConfigValue(configuration, "AuthSettings:Audience", "AUTH_AUDIENCE");

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: await BuildClaims(user),
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: GetSigningCredentials()
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<IEnumerable<Claim>> BuildClaims(User user)
        {
            var roleNames = await appDbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var tokenKey = ConfigurationHelper.GetConfigValue(configuration, "AuthSettings:Token", "AUTH_TOKEN");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        }
    }
}
