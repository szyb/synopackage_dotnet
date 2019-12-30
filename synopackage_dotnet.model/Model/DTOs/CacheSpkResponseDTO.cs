using System;
using System.Collections.Generic;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.DTOs
{
  public class CacheSpkResponseDTO
  {
    public bool Result { get; set; }
    public DateTime? CacheDate { get; set; }
    public double? CacheOld { get; set; }
    public SpkResult SpkResult { get; set; }

  }
}