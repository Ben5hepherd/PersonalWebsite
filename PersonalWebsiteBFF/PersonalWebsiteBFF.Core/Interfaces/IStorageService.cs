using Microsoft.AspNetCore.Http;

namespace PersonalWebsiteBFF.Core.Interfaces
{
    public interface IStorageService
    {
        public Task<string> UploadPhotoAsync(IFormFile file);
    }
}
