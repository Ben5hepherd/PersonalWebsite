using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Common.DTOs
{
    public class RegisterResultDto : ResultDto
    {
        public User? User { get; set; }
    }
}