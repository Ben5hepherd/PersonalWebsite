using Microsoft.AspNetCore.Http;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Core.Interfaces
{
    public interface IPhotoService
    {
        public Task<IEnumerable<Photo>> GetPhotosAsync();
        public Task UploadPhotoAsync(IFormFile file, string title, string description);
    }
}
