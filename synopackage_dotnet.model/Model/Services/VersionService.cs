using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class VersionService : IVersionService
  {
    public IEnumerable<VersionDTO> GetAllVersions() => VersionHelper.GetAllVersions();

    public VersionDTO GetVersion(string version) => VersionHelper.FindVersion(version); 
        
    public VersionDTO FindBestMatch(int build, int major, int minor, int micro)
    {
      var version = VersionHelper.FindByBuild(build);
      if (version != null)
        return version;

      var allVersions = VersionHelper.GetAllVersions();
      version = allVersions.Where(p => p.Major == major && p.Minor == minor && p.Micro == micro).OrderByDescending(p => p.Build).FirstOrDefault();
      if (version != null)
        return version;
      version = allVersions.Where(p => p.Major == major && p.Minor == minor).OrderByDescending(p => p.Build).FirstOrDefault();
      if (version != null)
        return version;

      version = allVersions.Where(p => p.Major == major).OrderByDescending(p => p.Build).FirstOrDefault();
      return version;
    }
  }
}
