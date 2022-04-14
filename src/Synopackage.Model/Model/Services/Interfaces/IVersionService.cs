using System.Collections.Generic;
using Synopackage.Model.DTOs;
namespace Synopackage.Model.Services
{
  public interface IVersionService : IDomainService
  {
    IEnumerable<VersionDTO> GetAllVersions();
    VersionDTO GetVersion(string version);
    VersionDTO FindBestMatch(int build, int major, int minor, int micro);
  }
}
