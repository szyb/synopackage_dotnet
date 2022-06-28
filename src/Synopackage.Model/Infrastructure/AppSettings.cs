using System;

namespace Synopackage
{
  public class AppSettings
  {
    public string DefaultModel { get; set; }
    public string DefaultVersion { get; set; }
    public string FrontendCacheFolder { get; set; }
    public string BackendCacheFolder { get; set; }
    [Obsolete("Use CacheOptionsManager")]
    public int? CacheIconExpirationInDays { get; set; }
    [Obsolete("Use CacheOptionsManager")]
    public bool CacheSpkServerResponse { get; set; }
    [Obsolete("Use CacheOptionsManager")]
    public int? CacheSpkServerResponseTimeInHours { get; set; }
    [Obsolete("Use CacheOptionsManager")]
    public int CacheSpkServerResponseTimeInHoursForRepository { get; set; } = 0;
    public int DownloadTimeoutInSeconds { get; set; }
    public int DownloadTimeoutInSecondsForRepository { get; set; }
    public string DownloadService { get; set; }
    public int DefaultItemsPerPage { get; set; }
    public bool EnableProxyDownloadForInsecureProtocol { get; set; }

    public HealthChecksSettings HealthChecks { get; set; }
  }
}
