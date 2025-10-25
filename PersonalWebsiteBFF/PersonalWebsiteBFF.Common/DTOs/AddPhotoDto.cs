using Microsoft.AspNetCore.Http;

namespace PersonalWebsiteBFF.Common.DTOs
{
    public class AddPhotoDto
    {
        public IFormFile File { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
