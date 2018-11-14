using System;

namespace synopackage_dotnet.Model.DTOs
{
  public class SearchLogEntryDTO
  {
    public DateTime Timestamp
    {
      get
      {
        return DateTime.Now;
      }
    }

    public string SourceContext { get; set; }
    public int ExecutionTime { get; set; }
    public ParametersDTO parameters { get; private set; }

    public SearchLogEntryDTO(ParametersDTO parameters)
    {
      this.parameters = parameters;
    }
  }
}