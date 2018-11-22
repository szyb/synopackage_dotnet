using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace synopackage_dotnet.Model.DTOs
{
  public class ChangelogDTO
  {
    public string Version { get; set; }
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public IEnumerable<string> New { get; set; }
    public IEnumerable<string> Improved { get; set; }
    public IEnumerable<string> Fixed { get; set; }
    public IEnumerable<string> RemovedSources { get; set; }

    public ChangelogDTO()
    {
      New = new List<string>();
      Improved = new List<string>();
      Fixed = new List<string>();
      RemovedSources = new List<string>();
    }
  }
}