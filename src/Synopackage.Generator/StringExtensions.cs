using System.Linq;

namespace Synopackage.Generator
{
  public static class StringExtensions
  {
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
          null => null,
          "" => "",
          _ => input.First().ToString().ToUpper() + input.Substring(1)
        };
  }
}
