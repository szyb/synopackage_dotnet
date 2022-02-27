using System.Collections.Generic;

namespace synopackage_dotnet.model.Model.DTOs
{
  public class RepositoryInfoDetailsDto
  {
    public string Url { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<string> Sources { get; set; }
    public RepositoryInfoDetailsDto(string url, string name, string description)
    {
      Url = url;
      Name = name;
      Description = description;
      Sources = new List<string>();
    }
  }
}