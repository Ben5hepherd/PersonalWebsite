using Microsoft.EntityFrameworkCore;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;

namespace PersonalWebsiteBFF.Core.Services
{
    public class PhotoService(AppDbContext appDbContext) : IPhotoService
    {
        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await appDbContext.Photos
                .OrderBy(p => p.Ordinal)
                .ToListAsync();
        }
    }
}
