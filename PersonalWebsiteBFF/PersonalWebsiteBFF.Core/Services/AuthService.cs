using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PersonalWebsiteBFF.Core.Services
{
    public class AuthService(
        IPasswordHasher<User> userPasswordHasher, 
        IJwtTokenService jwtTokenService,
        AppDbContext appDbContext) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await appDbContext.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new User(userPasswordHasher, request.Password, request.Username);

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

            var result = userPasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = await jwtTokenService.GenerateToken(user);

            return token;
        }
    }
}
