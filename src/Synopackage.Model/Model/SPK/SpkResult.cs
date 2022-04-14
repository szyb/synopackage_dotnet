using System.Collections.Generic;

namespace synopackage_dotnet.Model.SPK
{
  public class SpkResult
  {
    public SpkResult() { Keyrings = new List<string>(); Packages = new List<SpkPackage>(); }
    public List<string> Keyrings { get; set; }
    public List<SpkPackage> Packages { get; set; }
  }
}