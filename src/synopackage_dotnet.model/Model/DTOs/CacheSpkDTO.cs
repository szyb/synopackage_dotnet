using System;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.DTOs
{
  public class CacheSpkDTO
  {
    public DateTime? CacheDate { get; set; }
    public double? CacheOld { get; set; }
    public SpkResult SpkResult { get; set; }
  }
}
