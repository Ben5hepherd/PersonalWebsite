using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;

namespace PersonalWebsiteBFF.Core.Services
{
    public class PhotoService(AppDbContext appDbContext, IStorageService storageService) : IPhotoService
    {
        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await appDbContext.Photos
                .OrderBy(p => p.Ordinal)
                .ToListAsync();
        }

        public async Task UploadPhotoAsync(IFormFile file, string title, string description)
        {
            var photoPublicUrl = await storageService.UploadPhotoAsync(file);

            var maxOrdinal = await appDbContext.Photos.MaxAsync(p => (int?)p.Ordinal) ?? 0;

            var photoToAdd = new Photo
            {
                Url = photoPublicUrl,
                Title = title,
                Description = description,
                Ordinal = maxOrdinal + 1
            };

            await appDbContext.Photos.AddAsync(photoToAdd);
            await appDbContext.SaveChangesAsync();
        }
    }
}
