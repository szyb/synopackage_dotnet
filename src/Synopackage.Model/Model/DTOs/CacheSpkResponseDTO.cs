using System;
using System.Collections.Generic;
using Synopackage.Model.SPK;

namespace Synopackage.Model.DTOs
{
  public class CacheSpkResponseDTO
  {
    public bool HasValidCache { get; set; }
    public CacheSpkDTO Cache { get; set; }
  }
}
