namespace PersonalWebsiteBFF.Domain.Entities
{
    public class Role
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public const int UserId = 1;
        public const int AdminId = 2;

        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}
