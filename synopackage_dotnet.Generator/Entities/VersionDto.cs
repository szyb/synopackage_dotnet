using System;
using System.Text.RegularExpressions;

namespace synopackage_dotnet.Generator.Entities
{
  public class VersionDto
  {
    public string Version { get; set; }
  }

  public class ExtendedVersionDto : IComparable<ExtendedVersionDto>
  {
    static Regex regex = new Regex(@"^(?<major>\d)\.(?<minor>\d)(\.(?<micro>\d)){0,1}\-(?<build>(\d){1,5})$");
    public string Version { get; private set; }
    public string ShortVersion { get; private set; }
    public int Major { get; private set; }
    public int Minor { get; private set; }
    public int Micro { get; private set; }
    public int Build { get; private set; }
    public bool IsValid { get; private set; }

    public ExtendedVersionDto(string version)
    {
      Version = version;
      Match m = regex.Match(version);
      if (m.Success)
      {
        this.Major = Convert.ToInt32(m.Groups["major"].Value);
        this.Minor = Convert.ToInt32(m.Groups["minor"].Value);
        if (!string.IsNullOrWhiteSpace(m.Groups["micro"].Value))
        {
          this.Micro = Convert.ToInt32(m.Groups["micro"].Value);
          this.ShortVersion = $"{this.Major}.{this.Minor}.{this.Micro}";
        }
        else
          this.ShortVersion = $"{this.Major}.{this.Minor}";
        this.Build = Convert.ToInt32(m.Groups["build"].Value);
        IsValid = true;
      }
    }
    public int CompareTo(ExtendedVersionDto other)
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
