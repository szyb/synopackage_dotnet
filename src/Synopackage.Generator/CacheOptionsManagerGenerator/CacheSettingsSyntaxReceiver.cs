using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Synopackage.Generator.CacheOptionsManagerGenerator
{
  public class CacheSettingsSyntaxReceiver : ISyntaxReceiver
  {
    public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
      if (syntaxNode is ClassDeclarationSyntax cds &&
                cds.Identifier.ValueText == "CacheSettings")
      {
        foreach (var node in syntaxNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
        {
          if (node is PropertyDeclarationSyntax property && !Properties.ContainsKey(property.Identifier.ValueText))
          {
            Properties.Add(property.Identifier.ValueText, property.Type.ToString());
          }
        }
      }
    }
  }
}