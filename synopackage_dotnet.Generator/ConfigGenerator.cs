using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace synopackage_dotnet.Generator
{
  [Generator]
  public class ConfigGenerator : ISourceGenerator
  {
    IGeneratorHandler generatorHandler;
    public void Execute(GeneratorExecutionContext context)
    {
      var files = context.AdditionalFiles.Where(p => p.Path.EndsWith(".json"));
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file.Path);
        string generatedCode = GenerateClassesFromFile(file.Path);

        context.AddSource($"{fileName.FirstCharToUpper()}Helper.cs", SourceText.From(generatedCode, Encoding.UTF8));

      }
    }

    private string GenerateClassesFromFile(string path)
    {
      return generatorHandler.Handle(path);
    }

    public void Initialize(GeneratorInitializationContext context)
    {

      //if (!Debugger.IsAttached)
      //  Debugger.Launch();

      var sourcesGeneratorHandler = new SourcesGeneratorHandler();
      var modelsGeneratorHandler = new ModelsGeneratorHandler();
      var changelogsGeneratorHandler = new ChangelogsGeneratorHandler();
      var newsGeneratorHandler = new NewsGeneratorHandler();
      generatorHandler = sourcesGeneratorHandler;
      sourcesGeneratorHandler.SetupNext(modelsGeneratorHandler);
      modelsGeneratorHandler.SetupNext(changelogsGeneratorHandler);
      changelogsGeneratorHandler.SetupNext(newsGeneratorHandler);
    }
  }
}
