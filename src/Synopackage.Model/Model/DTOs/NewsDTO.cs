using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Synopackage.Model.DTOs
{
  public class NewsDTO
  {
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public List<string> Body { get; set; }
    public string RouterLinkDescription { get; set; }
    public string RouterLink { get; set; }
    public string ExternalLinkDescription { get; set; }
    public string ExternalLink { get; set; }
  }
}