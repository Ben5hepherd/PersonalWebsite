using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Core.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(User user);
    }
}
