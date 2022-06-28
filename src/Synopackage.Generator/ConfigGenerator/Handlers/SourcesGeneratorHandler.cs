using Newtonsoft.Json;
using Scriban;
using Synopackage.Generator.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Synopackage.Generator.ConfigGenerator.Handlers
{
  public class SourcesGeneratorHandler : GeneratorHandlerAbstract
  {
    public override string Handle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<IList<SourceDto>>(content);
      Template template = Template.Parse(GetFromResource("Synopackage.Generator.ConfigGenerator.Templates.Sources.sbncs"));
      FileInfo fi = new FileInfo(filePath);
      var rendered = template.Render(new
      {
        Sources = list,
        LastUpdateDate = fi.LastWriteTime
      });
      return rendered;
    }
  }
}
