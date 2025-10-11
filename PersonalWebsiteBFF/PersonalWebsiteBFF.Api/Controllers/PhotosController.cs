using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController(IPhotoService photoService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
        {
            var photos = await photoService.GetPhotosAsync();
            return Ok(photos);
        }
    }
}
