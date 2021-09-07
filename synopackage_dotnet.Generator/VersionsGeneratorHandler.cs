using Newtonsoft.Json;
using Scriban;
using synopackage_dotnet.Generator.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace synopackage_dotnet.Generator
{
  public class VersionsGeneratorHandler : GeneratorHandlerAbstract
  {
    public override bool CanHandle(string filePath)
    {
      string fileName = Path.GetFileNameWithoutExtension(filePath);
      return fileName == "versions";
    }

    public override string ConcreteHandle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<VersionDto>>(content);
      var extendedVersions = list
        .ConvertAll<ExtendedVersionDto>(p => new ExtendedVersionDto(p.Version))
        .Where(p => p.IsValid)
        .ToList();
      extendedVersions.Sort();

      Template template = Template.Parse(GetFromResource("synopackage_dotnet.Generator.Templates.Versions.sbncs"));
      var rendered = template.Render(new
      {
        Versions = extendedVersions
      });
      return rendered;
    }
  }
}
