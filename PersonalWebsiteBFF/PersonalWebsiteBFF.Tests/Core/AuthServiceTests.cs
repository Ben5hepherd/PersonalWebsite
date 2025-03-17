using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using PersonalWebsiteBFF.Core.Services;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using PersonalWebsiteBFF.Core.Interfaces;

namespace PersonalWebsiteBFF.Tests.Core
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<IPasswordHasher<User>> _userPasswordHasherMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly AppDbContext _dbContext;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _dbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("test").Options);
            _userPasswordHasherMock = new Mock<IPasswordHasher<User>>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _authService = new AuthService(_userPasswordHasherMock.Object, _jwtTokenServiceMock.Object, _dbContext);

            _userPasswordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedpassword");
        }

        public void Dispose()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task RegisterAsync_ReturnsNull_WhenUserAlreadyExists()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "Password123" };
            var user = new User(_userPasswordHasherMock.Object, "Password123", "existinguser");

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _authService.RegisterAsync(userDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsUser_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var userDto = new UserDto { Username = "newuser", Password = "Password123" };

            // Act
            var result = await _authService.RegisterAsync(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Username, result.Username);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            var userDto = new UserDto { Username = "nonexistentuser", Password = "Password123" };

            // Act
            var result = await _authService.LoginAsync(userDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "WrongPassword" };
            var user = new User(_userPasswordHasherMock.Object, "Password123", "existinguser");
            _dbContext.Users.Add(user);

            // Act
            var result = await _authService.LoginAsync(userDto);

            // Assert
            Assert.Null(result);
        }
    }
}
