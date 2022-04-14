namespace Synopackage
{
  public class AppSettingsProvider
  {
    public static AppSettings AppSettings { get; private set; }

    private AppSettingsProvider(AppSettings appSettings)
    {
      AppSettings = appSettings;
    }

    public static AppSettingsProvider Create(AppSettings appSetting)
    {
      return new AppSettingsProvider(appSetting);
    }

  }
}
