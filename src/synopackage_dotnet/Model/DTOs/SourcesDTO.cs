using System.Collections.Generic;

namespace synopackage_dotnet.Model.DTOs
{
  public class SourcesDTO
  {
    public SourcesDTO()
    {
      this.ActiveSources = new List<SourceDTO>();
      this.InActiveSources = new List<SourceDTO>();
    }
    public List<SourceDTO> ActiveSources { get; set; }
    public List<SourceDTO> InActiveSources { get; set; }
  }
}
