using System;

namespace synopackage_dotnet.Model.DTOs
{
  public class VersionDTO : IComparable<VersionDTO>
  {
    public string Version { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Micro { get; set; }
    public int Build { get; set; }
    public int CompareTo(VersionDTO other)
    {
      if (other == null)
        return 1;
      var compare = this.Major.CompareTo(other.Major);
      if (compare != 0) return -compare;
      compare = this.Minor.CompareTo(other.Minor);
      if (compare != 0) return -compare;
      compare = this.Micro.CompareTo(other.Micro);
      if (compare != 0) return -compare;
      compare = this.Build.CompareTo(other.Build);
      if (compare != 0) return -compare;
      return -this.Version.CompareTo(other.Version);
    }

    public override string ToString()
    {
      if (Micro != 0)
        return $"{Major}.{Minor}.{Micro}-{Build}";
      else
        return $"{Major}.{Minor}-{Build}";
    }
  }
}
