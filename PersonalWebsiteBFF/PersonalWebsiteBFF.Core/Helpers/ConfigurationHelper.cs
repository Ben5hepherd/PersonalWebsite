using Microsoft.Extensions.Configuration;

namespace PersonalWebsiteBFF.Core.Helpers
{
    public static class ConfigurationHelper
    {
        public static string GetConfigValue(IConfiguration configuration, string local, string deployed)
        {
            return configuration.GetValue<string>(local) ?? Environment.GetEnvironmentVariable(deployed)!;
        }
    }
}
