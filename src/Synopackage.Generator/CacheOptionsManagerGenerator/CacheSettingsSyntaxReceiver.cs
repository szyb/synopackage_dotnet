using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Synopackage.Generator.CacheOptionsManagerGenerator
{
  public class CacheSettingsSyntaxReceiver : ISyntaxReceiver
  {
    public Dictionary<string, GeneratorPropertyDescriptor> Properties { get; } = new Dictionary<string, GeneratorPropertyDescriptor>();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
      if (syntaxNode is ClassDeclarationSyntax cds &&
                cds.Identifier.ValueText == "CacheSettings")
      {
        foreach (var node in syntaxNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
        {
          if (node is PropertyDeclarationSyntax property && !Properties.ContainsKey(property.Identifier.ValueText))
          {
            bool allowNullForDefaults = false;
            allowNullForDefaults = node.AttributeLists.Any(p => p.Attributes.Any(x => x.Name.ToString() == "AllowNullForDefaults"));
            GeneratorPropertyDescriptor descriptor = new GeneratorPropertyDescriptor()
            {
              PropertyName = property.Identifier.ValueText,
              PropertyType = property.Type.ToString(),
              AllowNullForDefaults = allowNullForDefaults
            };
            if (!allowNullForDefaults && descriptor.PropertyType.EndsWith("?"))
              descriptor.PropertyType = descriptor.PropertyType.Substring(0, descriptor.PropertyType.Length - 1);
            Properties.Add(property.Identifier.ValueText, descriptor);
          }
        }
      }
    }
  }
}