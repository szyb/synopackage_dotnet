using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Synopackage.Generator.CacheOptionsManagerGenerator
{
  [Generator]
  public class CacheOptionsManagerGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
      //if (!Debugger.IsAttached)
      //  Debugger.Launch();
      var syntaxReceiver = new CacheSettingsSyntaxReceiver();
      context.RegisterForSyntaxNotifications(() => syntaxReceiver);

    }
    public void Execute(GeneratorExecutionContext context)
    {
      if (context.SyntaxReceiver is not CacheSettingsSyntaxReceiver receiver)
        return;
      Template template = Template.Parse(GetFromResource("Synopackage.Generator.CacheOptionsManagerGenerator.Templates.CacheOptionsManager.sbncs"));
      var rendered = template.Render(new
      {
        Properties = receiver.Properties.Values
      });

      context.AddSource("CacheOptionsManager.g.cs", SourceText.From(rendered, Encoding.UTF8));

      template = Template.Parse(GetFromResource("Synopackage.Generator.CacheOptionsManagerGenerator.Templates.ICacheOptionsManager.sbncs"));
      rendered = template.Render(new
      {
        Properties = receiver.Properties.Values
      });

      context.AddSource("ICacheOptionsManager.g.cs", SourceText.From(rendered, Encoding.UTF8));
    }

    private static readonly Assembly assembly = typeof(CacheOptionsManagerGenerator).Assembly;
    protected string GetFromResource(string name)
    {
      using (Stream stream = assembly.GetManifestResourceStream(name))
      using (StreamReader reader = new StreamReader(stream))
      {
        string result = reader.ReadToEnd();
        return result;
      }
    }

  }
}
