using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteBFF.Common.DTOs;
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

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> UploadPhoto([FromForm] AddPhotoDto form)
        {
            await photoService.UploadPhotoAsync(form.File, form.Title, form.Description);
            return Ok();
        }
    }
}
