using System.Collections.Generic;
using System.Threading.Tasks;
using Synopackage.Model.DTOs;
using Synopackage.Model.SPK;

namespace Synopackage.Model.Services
{
  public interface ICacheService : IDomainService
  {
    Task ProcessIcons(string sourceName, List<SpkPackage> packages);
    string GetIconFileName(string sourceName, string packageName);
    string GetIconFileNameWithCacheFolder(string sourceName, string packageName);
    Task<CacheSpkResponseDTO> GetSpkResponseFromCache(string sourceName, string arch, string model, VersionDTO version, bool isBeta);
    Task<CacheSpkResponseDTO> GetSpkResponseForRepositoryFromCache(string sourceName, string arch, VersionDTO version, bool isBeta);
    bool SaveSpkResult(string sourceName, string arch, string model, VersionDTO version, bool isBeta, SpkResult spkResult);

  }
}
