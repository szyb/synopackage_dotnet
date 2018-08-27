using System.IO;
using System.Linq;

namespace synopackage_dotnet
{
  public static class Utils
  {
    public static string CleanFileName(string fileName) 
    { 
      return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_")); 
    }
  }
}