using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Interfaces;

namespace PersonalWebsiteBFF.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResultDto>> Register(UserDto userDto)
        {
            var result = await authService.RegisterAsync(userDto);

            if (result.Success && result.User != null)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto userDto)
        {
            var token = await authService.LoginAsync(userDto);

            if (token is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
