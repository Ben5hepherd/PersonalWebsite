using PersonalWebsite.Api.Entities;
using PersonalWebsite.Api.Models;

namespace PersonalWebsite.Api.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
    }
}
