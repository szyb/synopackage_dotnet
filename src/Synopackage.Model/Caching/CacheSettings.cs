using Synopackage.Model.Caching.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching
{
  public class CacheSettings
  {
    /// <summary>
    /// Cache icon expiration in days. When null - expiration is disabled
    /// </summary>
    [AllowNullForDefaults]
    public int? CacheIconExpirationInDays { get; set; }
    /// <summary>
    /// Indicates whether response should be cached
    /// </summary>
    public bool? CacheSpkServerResponse { get; set; }
    /// <summary>
    /// Cache expiration (in hours)
    /// </summary>
    public int? CacheSpkServerResponseTimeInHours { get; set; }
    /// <summary>
    /// Cache expiration for repository (in hours)
    /// </summary>
    public int? CacheSpkServerResponseTimeInHoursForRepository { get; set; }
    /// <summary>
    /// CPU architecture cache level
    /// </summary>
    public ArchCacheLevel? ArchCacheLevel { get; set; }
    /// <summary>
    /// CPU architecture list for ArchCacheLevel.OnlyListed option
    /// </summary>
    [AllowNullForDefaults]
    public List<string> ArchList { get; set; }
    /// <summary>
    /// DSM Version cache level
    /// </summary>
    public VersionCacheLevel? VersionCacheLevel { get; set; }
    /// <summary>
    /// Channel cache level
    /// </summary>
    public ChannelCacheLevel? ChannelCacheLevel { get; set; }
  }
}
