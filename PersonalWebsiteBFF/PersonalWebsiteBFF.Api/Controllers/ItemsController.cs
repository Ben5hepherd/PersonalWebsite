using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = Role.Member)]
        public async Task<ActionResult<IEnumerable<string>>> GetItems()
        {
            List<string> items = ["item1", "item2", "item3"];
            return Ok(items);
        }
    }
}
