using System.Collections.Generic;

namespace synopackage_dotnet.Model.SPK
{
  public class SpkPackage
  {
    public SpkPackage() { }

    //use only for deserialization. Use Name instead
    public string Dname { get; set; }
    public string Desc { get; set; }
    public string Version { get; set; }
    public string Package { get; set; }
    public string Link { get; set; }
    public bool Beta { get; set; }
    public List<string> Thumbnail { get; set; }
    public string Icon { get; set; }
    public string Name
    {
      get
      {
        return Dname != null ? Dname : Package;
      }
    }
    //TODO: implement other if needed
  }


}