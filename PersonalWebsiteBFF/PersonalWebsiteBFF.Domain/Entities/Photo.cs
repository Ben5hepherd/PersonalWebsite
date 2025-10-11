namespace PersonalWebsiteBFF.Domain.Entities
{
    public class Photo
    {
        public required int Id { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
