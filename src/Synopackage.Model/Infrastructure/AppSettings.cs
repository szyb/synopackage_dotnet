using System;

namespace Synopackage
{
  public class AppSettings
  {
    public string DefaultModel { get; set; }
    public string DefaultVersion { get; set; }
    public string FrontendCacheFolder { get; set; }
    public string BackendCacheFolder { get; set; }
    public int DownloadTimeoutInSeconds { get; set; }
    public int DownloadTimeoutInSecondsForRepository { get; set; }
    public string DownloadService { get; set; }
    public int DefaultItemsPerPage { get; set; }
    public bool EnableProxyDownloadForInsecureProtocol { get; set; }
    public bool ShouldProcessIcons { get; set; } = true;

    public HealthChecksSettings HealthChecks { get; set; }
  }
}
