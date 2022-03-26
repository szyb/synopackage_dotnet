using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synopackage_dotnet.model
{
  public static class FileInfoExtensions
  {
    [Obsolete("Use static method IsCacheFileExpired im CacheService")]
    public static bool IsCacheFileExpired(this FileInfo fi, int cacheValidTimeInHours)
    {
      if (!fi.Exists)
        return true;
      else
        return (DateTime.Now - fi.LastWriteTime).TotalHours > cacheValidTimeInHours;
    }
  }
}
