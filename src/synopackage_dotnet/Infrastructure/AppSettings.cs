namespace synopackage_dotnet
{
  public class AppSettings
  {
    public string ProxyUrl { get; set; }
    public string FrontendCacheFolder { get; set; }
    public string BackendCacheFolder { get; set; }
    public int? CacheIconExpirationInDays { get; set; }
    public bool CacheSpkServerResponse { get; set; }
    public int? CacheSpkServerResponseTimeInHours { get; set; }
  }
}