using System;

namespace synopackage_dotnet.Model.DTOs
{
  public class PackageDTO : IComparable<PackageDTO>
  {
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Version { get; set; }
    public string Package { get; set; }
    public string Description { get; set; }
    public bool IsBeta { get; set; }
    public string DownloadLink { get; set; }
    public string IconFileName { get; set; }
    public string SourceName { get; set; }

    public int CompareTo(PackageDTO other)
    {
      if (other == null)
        return 1;
      var comp = this.Name.CompareTo(other.Name);
      if (comp != 0)
        return comp;
      comp = this.Version.CompareTo(other.Version);
      return comp;
    }
  }
}
