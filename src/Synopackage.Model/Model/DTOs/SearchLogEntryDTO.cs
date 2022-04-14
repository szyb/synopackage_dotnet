using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using synopackage_dotnet.Model.Enums;
using System;

namespace synopackage_dotnet.Model.DTOs
{


  public class SearchLogEntryDTO
  {
    public Guid Id { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public LogType LogType { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ResultFrom ResultFrom { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public RequestType RequestType { get; set; }
    public ParametersDTO parameters { get; set; }
    public double? CacheOld { get; set; }
    public long ExecutionTime { get; set; }

    public SearchLogEntryDTO(ParametersDTO parameters)
    {
      this.Id = Guid.NewGuid();
      this.LogType = LogType.Parameters;
      this.parameters = parameters;
    }
  }
}
