using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PersonalWebsiteBFF.Core.Services
{
    public class AuthService(IConfiguration configuration, AppDbContext appDbContext) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await appDbContext.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new User();

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            appDbContext.Users.Add(user);

            var userRole = new UserRole
            {
                RoleId = Role.MemberId,
                UserId = user.Id
            };
            appDbContext.UserRoles.Add(userRole);

            await appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null)
            {
                return null;
            }

            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = await CreateToken(user);

            return token;
        }

        private async Task<string> CreateToken(User user)
        {
            var tokenKey = configuration.GetValue<string>("AuthSettings:Token")
                ?? Environment.GetEnvironmentVariable("AUTH_TOKEN")!;

            var issuer = configuration.GetValue<string>("AuthSettings:Issuer")
                ?? Environment.GetEnvironmentVariable("AUTH_ISSUER");

            var audience = configuration.GetValue<string>("AuthSettings:Audience")
                ?? Environment.GetEnvironmentVariable("AUTH_AUDIENCE");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

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

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
