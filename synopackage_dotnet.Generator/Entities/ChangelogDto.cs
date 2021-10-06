using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace synopackage_dotnet.Generator.Entities
{
  public class ChangelogDto
  {
    public string Version { get; set; }
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public IEnumerable<string> New { get; set; }
    public IEnumerable<string> Improved { get; set; }
    public IEnumerable<string> Fixed { get; set; }
    public IEnumerable<string> RemovedSources { get; set; }

    public ChangelogDto()
    {
      New = new List<string>();
      Improved = new List<string>();
      Fixed = new List<string>();
      RemovedSources = new List<string>();
    }
  }
}
