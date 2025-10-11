using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Core.Interfaces
{
    public interface IPhotoService
    {
        public Task<IEnumerable<Photo>> GetPhotosAsync();
    }
}
