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
    [AllowNullForDefaults]
    public int? CacheIconExpirationInDays { get; set; }
    public bool? CacheSpkServerResponse { get; set; }
    public int? CacheSpkServerResponseTimeInHours { get; set; }
    public int? CacheSpkServerResponseTimeInHoursForRepository { get; set; }
    public ArchCacheLevel? ArchCacheLevel { get; set; }
    [AllowNullForDefaults]
    public List<string> ArchList { get; set; }
    public VersionCacheLevel? VersionCacheLevel { get; set; }
    public ChannelCacheLevel? ChannelCacheLevel { get; set; }
  }
}
