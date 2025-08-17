using PersonalWebsiteBFF.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteBFF.Common.DTOs
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StrongPassword]
        public string Password { get; set; } = string.Empty;
    }
}
