using System.Collections.Generic;
using System.Threading.Tasks;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public interface ICacheService : IDomainService
  {
    Task ProcessIcons(string sourceName, List<SpkPackage> packages);
    string GetIconFileName(string sourceName, string packageName);
    string GetIconFileNameWithCacheFolder(string sourceName, string packageName);
    Task<CacheSpkResponseDTO> GetSpkResponseFromCache(string sourceName, string arch, string model, string version, bool isBeta);
    Task<CacheSpkResponseDTO> GetSpkResponseForRepositoryFromCache(string sourceName, string arch, string version, bool isBeta);
    bool SaveSpkResult(string sourceName, string arch, string model, string version, bool isBeta, SpkResult spkResult);

  }
}
