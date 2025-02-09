using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using PersonalWebsiteBFF.Core.Services;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace PersonalWebsiteBFF.Tests.Core
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AppDbContext _dbContext;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _dbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("test").Options);
            _configurationMock = new Mock<IConfiguration>();
            _authService = new AuthService(_configurationMock.Object, _dbContext);
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

            _dbContext.Users.Add(new User { Id = new Guid(), Username = "existinguser" });
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
            _dbContext.Users.Add(new User { Id = new Guid(), Username = "existinguser", PasswordHash = "hashedpassword" });

            // Act
            var result = await _authService.LoginAsync(userDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenPasswordIsCorrect()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "Password123" };

            var user = new User { Id = new Guid(), Username = "existinguser", PasswordHash = new PasswordHasher<User>().HashPassword(null, "Password123") };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var mockTokenSection = new Mock<IConfigurationSection>();
            mockTokenSection.Setup(s => s.Value).Returns("mysecretkey1111111111111111111111111111111111111111111111111111111111111111111111111111");

            var mockIssuerSection = new Mock<IConfigurationSection>();
            mockIssuerSection.Setup(s => s.Value).Returns("testissuer");

            var mockAudienceSection = new Mock<IConfigurationSection>();
            mockAudienceSection.Setup(s => s.Value).Returns("testaudience");

            _configurationMock.Setup(c => c.GetSection("AuthSettings:Token")).Returns(mockTokenSection.Object);
            _configurationMock.Setup(c => c.GetSection("AuthSettings:Issuer")).Returns(mockIssuerSection.Object);
            _configurationMock.Setup(c => c.GetSection("AuthSettings:Audience")).Returns(mockAudienceSection.Object);

            // Act
            var result = await _authService.LoginAsync(userDto);

            // Assert
            Assert.NotNull(result);
        }
    }
}
