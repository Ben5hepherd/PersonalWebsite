using System.Text.RegularExpressions;

namespace PersonalWebsiteBFF.Common.Validators
{
    public static class PasswordStrengthValidator
    {
        public static bool IsStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            var hasUpper = Regex.IsMatch(password, "[A-Z]");
            var hasLower = Regex.IsMatch(password, "[a-z]");
            var hasDigit = Regex.IsMatch(password, "[0-9]");
            var hasSpecial = Regex.IsMatch(password, "[^a-zA-Z0-9]");

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}