using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Icon { get; set; }
    public string Name
    {
      get
      {
        return Dname ?? Package;
      }
    }
    public long? Size { get; set; }
    public string Changelog { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Conflictpkgs { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Deppkgs { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Depsers { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Thirdparty { get; set; }
    public string Distributor { get; set; }
    public string Distributor_url { get; set; }
    public long? Download_count { get; set; }
    public string Maintainer { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Qinst { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Qstart { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Qupgrade { get; set; }
    public long? Recent_download_count { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Category { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Subcategory { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Type { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Price { get; set; }
    public List<string> Thumbnail_retina { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Snapshot { get; set; }
    public string Md5 { get; set; }
    public bool? Start { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Silent_install { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Silent_upgrade { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Silent_uninstall { get; set; }

  }


}
