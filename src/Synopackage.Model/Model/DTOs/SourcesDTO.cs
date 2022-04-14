using System;
using System.Collections.Generic;

namespace Synopackage.Model.DTOs
{
  public class SourcesDTO
  {
    public SourcesDTO()
    {
      this.ActiveSources = new List<SourceDTO>();
      this.InactiveSources = new List<SourceDTO>();
    }
    public IList<SourceDTO> ActiveSources { get; set; }
    public IList<SourceDTO> InactiveSources { get; set; }
    public DateTime LastUpdateDate { get; set; }
  }
}
