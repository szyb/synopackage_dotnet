using System;
using Newtonsoft.Json;

namespace Synopackage.Model.DTOs
{
  public class SourceDTO
  {
    public bool Active { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Www { get; set; }
    public string DisabledReason { get; set; }
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime? DisabledDate { get; set; }
    public string CustomUserAgent { get; set; }
    public bool UseGetMethod { get; set; }
    public bool IsOfficial { get; set; }
    public string DisplayUrl { get; set; }
    public int? MinMajorVersion { get; set; }
    public int? MaxMajorVersion { get; set; }
    public bool? SynopackageChoice { get; set; }
    public bool? IsHealthy { get; set; }
    public string HealthCheckDescription { get; set; }
    public string Info { get; set; }
    public bool IsDownloadDisabled { get; set; }
  }
}
