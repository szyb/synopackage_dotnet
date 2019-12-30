namespace synopackage_dotnet
{  
  public class AppSettingsProvider
  {
    public static AppSettings AppSettings { get; set; }

    public AppSettingsProvider(AppSettings appSettings)
    {
      AppSettings = appSettings;
    }

  }
}