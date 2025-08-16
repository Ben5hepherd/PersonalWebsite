namespace PersonalWebsiteBFF.Domain.Entities
{
    public class UserRole
    {
        public required Guid UserId { get; set; }
        public required int RoleId { get; set; }

        public required User User { get; set; }
        public required Role Role { get; set; }
    }
}
