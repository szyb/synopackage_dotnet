using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Caching
{
  public class CacheSettings
  {
    public int? CacheIconExpirationInDays { get; set; }
    public bool? CacheSpkServerResponse { get; set; }
    public int? CacheSpkServerResponseTimeInHours { get; set; }
    public int? CacheSpkServerResponseTimeInHoursForRepository { get; set; }
  }
}
