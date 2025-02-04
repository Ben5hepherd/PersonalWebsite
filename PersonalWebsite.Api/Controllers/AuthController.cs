﻿using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.Entities;
using PersonalWebsite.Api.Models;
using PersonalWebsite.Api.Services;

namespace PersonalWebsite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if(user is null)
            {
                return BadRequest("User already exists");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await authService.LoginAsync(request);

            if (token is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
