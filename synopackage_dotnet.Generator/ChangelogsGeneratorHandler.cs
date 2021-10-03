using Newtonsoft.Json;
using Scriban;
using synopackage_dotnet.Generator.Entities;
using System.Collections.Generic;
using System.IO;

namespace synopackage_dotnet.Generator
{
  public class ChangelogsGeneratorHandler : GeneratorHandlerAbstract
  {
    public override bool CanHandle(string filePath)
    {
      string fileName = Path.GetFileNameWithoutExtension(filePath);
      return fileName == "changelog";
    }

    public override string ConcreteHandle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<ChangelogDto>>(content);
      Template template = Template.Parse(GetFromResource("synopackage_dotnet.Generator.Templates.Changelogs.sbncs"));
      var rendered = template.Render(new
      {
        Changelogs = list
      });
      return rendered;
    }
  }
}
