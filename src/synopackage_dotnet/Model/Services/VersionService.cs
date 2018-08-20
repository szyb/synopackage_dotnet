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
        public IEnumerable<VersionDTO> GetAllVersions()
        {
            var versionsJson = File.ReadAllText("Config/versions.json");
            var versions = JsonConvert.DeserializeObject<VersionDTO[]>(versionsJson);
            Regex regex = new Regex(@"^(?<major>\d)\.(?<minor>\d)(\.(?<micro>\d)){0,1}\-(?<build>(\d){1,5})$");
            foreach (var version in versions)
            {
                Match m = regex.Match(version.Version);
                if (m.Success)
                {
                    version.Major = Convert.ToInt32(m.Groups["major"].Value);
                    version.Minor = Convert.ToInt32(m.Groups["minor"].Value);
                    if (!string.IsNullOrWhiteSpace(m.Groups["micro"].Value))
                        version.Micro = Convert.ToInt32(m.Groups["micro"].Value);
                    version.Build = Convert.ToInt32(m.Groups["build"].Value);
                }
            }
            var list = versions.ToList();
            list.Sort();
            return list;
        }
    }
}
