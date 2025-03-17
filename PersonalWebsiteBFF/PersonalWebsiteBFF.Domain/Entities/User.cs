using Microsoft.AspNetCore.Identity;

namespace PersonalWebsiteBFF.Domain.Entities
{
    public class User
    {
        // Required for EF Core
        protected User() { }

        public User(IPasswordHasher<User> userPasswordHasher, string password, string username)
        {
            PasswordHash = userPasswordHasher.HashPassword(this, password);
            Username = username;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Username { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
    }
}
