using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Services;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;

namespace PersonalWebsiteBFF.Tests.Core
{
    public class JwtTokenServiceTests : IDisposable
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IPasswordHasher<User>> _userPasswordHasherMock;
        private readonly AppDbContext _dbContext;
        private readonly JwtTokenService _jwtTokenService;

        public JwtTokenServiceTests()
        {
            _dbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("test").Options);
            _configurationMock = new Mock<IConfiguration>();
            _jwtTokenService = new JwtTokenService(_configurationMock.Object, _dbContext);

            _userPasswordHasherMock = new Mock<IPasswordHasher<User>>();
            _userPasswordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedpassword");
        }

        public void Dispose()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GenerateToken_ReturnsToken_WhenPasswordIsCorrect()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "Password123" };
            var user = new User(_userPasswordHasherMock.Object, "Password123", "existinguser");

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
            var result = await _jwtTokenService.GenerateToken(user);

            // Assert
            Assert.NotNull(result);
        }
    }
}
