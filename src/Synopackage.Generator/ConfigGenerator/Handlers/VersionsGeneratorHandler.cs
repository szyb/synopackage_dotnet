﻿using Newtonsoft.Json;
using Scriban;
using Synopackage.Generator.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Synopackage.Generator.ConfigGenerator.Handlers
{
  public class VersionsGeneratorHandler : GeneratorHandlerAbstract
  {
    public override string Handle(string filePath)
    {
      var content = File.ReadAllText(filePath);
      var list = JsonConvert.DeserializeObject<List<VersionDto>>(content);
      var extendedVersions = list
        .ConvertAll(p => new ExtendedVersionDto(p.Version))
        .Where(p => p.IsValid)
        .ToList();
      extendedVersions.Sort();

      Template template = Template.Parse(GetFromResource("Synopackage.Generator.ConfigGenerator.Templates.Versions.sbncs"));
      var rendered = template.Render(new
      {
        Versions = extendedVersions
      });
      return rendered;
    }
  }
}
