using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalWebsiteBFF.Api.Controllers;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Tests.Api
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsCreated()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "Password123" };
            var user = new User { Id = new Guid(), Username = "testuser" };
            _authServiceMock.Setup(service => service.RegisterAsync(userDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Username, returnedUser.Username);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "Password123" };
            _ = _authServiceMock.Setup(service => service.RegisterAsync(userDto)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("User already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValidCredentials()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "Password123" };
            var token = "valid_token";
            _authServiceMock.Setup(service => service.LoginAsync(userDto)).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedToken = Assert.IsType<string>(okResult.Value);
            Assert.Equal(token, returnedToken);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenInvalidCredentials()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "WrongPassword" };
            _authServiceMock.Setup(service => service.LoginAsync(userDto)).ReturnsAsync((string)null);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid username or password", badRequestResult.Value);
        }
    }
}
