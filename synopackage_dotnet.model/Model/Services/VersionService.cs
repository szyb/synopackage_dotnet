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
  }
}
