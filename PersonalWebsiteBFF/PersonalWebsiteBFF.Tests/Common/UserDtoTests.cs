using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PersonalWebsiteBFF.Common.DTOs;

namespace PersonalWebsiteBFF.Tests.Common
{
    public class UserDtoTests
    {
        [Theory]
        [InlineData("Password123!")] // valid
        [InlineData("StrongPass1@")] // valid
        public void UserDto_Password_IsValid_WhenStrong(string strongPassword)
        {
            var dto = new UserDto { Username = "user", Password = strongPassword };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.True(isValid);
            Assert.DoesNotContain(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains("Password is not strong enough"));
        }

        [Theory]
        [InlineData("short1!")] // too short
        [InlineData("nouppercase1!")] // no uppercase
        [InlineData("NOLOWERCASE1!")] // no lowercase
        [InlineData("NoSpecial123")] // no special char
        [InlineData("NoDigit!")] // no digit
        public void UserDto_Password_IsInvalid_WhenWeak(string weakPassword)
        {
            var dto = new UserDto { Username = "user", Password = weakPassword };
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(dto, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains("Password is not strong enough"));
        }
    }
}