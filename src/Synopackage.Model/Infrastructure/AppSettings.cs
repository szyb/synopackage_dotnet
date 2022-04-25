namespace Synopackage
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
    public int CacheSpkServerResponseTimeInHoursForRepository { get; set; } = 0;
    public int DownloadTimeoutInSeconds { get; set; }
    public int DownloadTimeoutInSecondsForRepository { get; set; }
    public string DownloadService { get; set; }
    public int DefaultItemsPerPage { get; set; }
    public bool EnableProxyDownloadForInsecureProtocol { get; set; }

    public HealthChecksSettings HealthChecks { get; set; }
  }
}
