using System;
using System.Collections.Generic;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.DTOs
{
  public class CacheSpkResponseDTO
  {
    public bool HasValidCache { get; set; }
    public CacheSpkDTO Cache { get; set; }
  }
}
