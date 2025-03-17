namespace PersonalWebsiteBFF.Domain.Entities
{
    public class UserRole
    {
        public required Guid UserId { get; set; }
        public required int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
