using ConsoleApp1.Configuration;

namespace ConsoleApp1;

public static class ConfigManager
{
    private static AppSettings _config;

    public static AppSettings GetConfig()
    {
        if (_config == null)
            _config = ConfigLoader.LoadConfig(@"configuration/appSetting.json");
        return _config;
    }
}