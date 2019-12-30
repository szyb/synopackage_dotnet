namespace synopackage_dotnet
{
  public class AppSettings
  {
    public string DefaultModel { get; set; }
    public string DefaultVersion { get; set; }
    public string FrontendCacheFolder { get; set; }
    public string BackendCacheFolder { get; set; }
    public int? CacheIconExpirationInDays { get; set; }
    public bool CacheSpkServerResponse { get; set; }
    public int? CacheSpkServerResponseTimeInHours { get; set; }
    public int DownloadTimeoutInSeconds { get; set; }
    public string DownloadService { get; set; }
  }
}
