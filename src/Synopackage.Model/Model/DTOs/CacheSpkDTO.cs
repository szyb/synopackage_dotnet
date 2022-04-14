using System;
using Synopackage.Model.SPK;

namespace Synopackage.Model.DTOs
{
  public class CacheSpkDTO
  {
    public DateTime? CacheDate { get; set; }
    public double? CacheOld { get; set; }
    public SpkResult SpkResult { get; set; }
  }
}
