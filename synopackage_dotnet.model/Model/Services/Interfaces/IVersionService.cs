using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;
namespace synopackage_dotnet.Model.Services
{
    public interface IVersionService : IDomainService 
    {
        IEnumerable<VersionDTO> GetAllVersions();
        VersionDTO GetVersion(string version);
        VersionDTO FindBestMatch(int build, int major, int minor, int micro);
    }
}
