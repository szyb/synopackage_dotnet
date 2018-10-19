using System;
using Newtonsoft.Json;

namespace synopackage_dotnet.Model.DTOs
{
  public class NewsDTO
  {
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
  }
}