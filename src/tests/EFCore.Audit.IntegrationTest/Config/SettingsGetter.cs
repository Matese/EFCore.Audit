using EFCore.Audit.IntegrationTest;
using Microsoft.Extensions.Configuration;

namespace EFCore.Audit.IntegrationTest.Config
{
    public static class SettingsGetter
    {
        public static Settings Get()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();

            var settings = config
                .GetSection("Settings")
                .Get<Settings>();

            return settings;
        }
    }
}
