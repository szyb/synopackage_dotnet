using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
    public class VersionService : IVersionService
    {
        public IEnumerable<VersionDTO> GetAllVersions()
        {
            //TODO: implement
            List<VersionDTO> list = new List<VersionDTO>();
            list.Add(new VersionDTO() {Version = "6.2"});
            list.Add(new VersionDTO() {Version = "6.1"});
            return list;
        }
    }
}
