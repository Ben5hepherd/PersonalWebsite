using PersonalWebsiteBFF.Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteBFF.Common.Attributes
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public StrongPasswordAttribute()
        {
            ErrorMessage = "Password is not strong enough. It must be at least 8 characters long and include uppercase, lowercase, digit, and special character.";
        }

        public override bool IsValid(object? value)
        {
            var password = value as string;
            var result = PasswordStrengthValidator.IsStrong(password ?? string.Empty);
            return result;
        }
    }
}