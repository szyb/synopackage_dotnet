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
    public void Execute(GeneratorExecutionContext context)
    {
      var files = context.AdditionalFiles.Where(p => p.Path.EndsWith(".json"));
      foreach (var file in files)
      {
        var fileName = Path.GetFileNameWithoutExtension(file.Path);
        string generatedCode = GenerateClassesFromFile(file.Path);

        context.AddSource($"{fileName}.cs", SourceText.From(generatedCode, Encoding.UTF8));

      }
    }

    private string GenerateClassesFromFile(string path)
    {
      return null;
    }

    public void Initialize(GeneratorInitializationContext context)
    {
      //no need to initialize
    }
  }
}
