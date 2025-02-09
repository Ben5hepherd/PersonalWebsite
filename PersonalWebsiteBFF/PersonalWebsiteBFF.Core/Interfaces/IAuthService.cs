using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
    }
}
