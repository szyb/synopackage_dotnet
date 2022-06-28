using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Synopackage.Generator.ConfigGenerator.Handlers;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Synopackage.Generator.ConfigGenerator
{
  [Generator]
  public class ConfigGenerator : ISourceGenerator
  {
    private readonly IGeneratorHandler sourcesGeneratorHandler = new SourcesGeneratorHandler();
    private readonly IGeneratorHandler versionsGeneratorHandler = new VersionsGeneratorHandler();
    private readonly IGeneratorHandler modelsGeneratorHandler = new ModelsGeneratorHandler();
    private readonly IGeneratorHandler changelogsGeneratorHandler = new ChangelogsGeneratorHandler();
    private readonly IGeneratorHandler newsGeneratorHandler = new NewsGeneratorHandler();
    public void Execute(GeneratorExecutionContext context)
    {
      var files = context.AdditionalFiles.Where(p => p.Path.EndsWith(".json")).Select(p => p.Path);
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file);
        string generatedCode = GenerateClassesFromFile(file);
        if (generatedCode != null)
          context.AddSource($"{fileName.FirstCharToUpper()}Helper.g.cs", SourceText.From(generatedCode, Encoding.UTF8));

      }
    }

    private string GenerateClassesFromFile(string path)
    {
      string fileName = Path.GetFileNameWithoutExtension(path);
      return fileName switch
      {
        "sources" => sourcesGeneratorHandler.Handle(path),
        "versions" => versionsGeneratorHandler.Handle(path),
        "models" => modelsGeneratorHandler.Handle(path),
        "changelog" => changelogsGeneratorHandler.Handle(path),
        "news" => newsGeneratorHandler.Handle(path),
        _ => null,
      };
    }

    public void Initialize(GeneratorInitializationContext context)
    {

      //if (!Debugger.IsAttached)
      //  Debugger.Launch();


    }
  }
}
