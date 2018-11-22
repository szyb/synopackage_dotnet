using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public interface ICacheService : IDomainService
  {
    void ProcessIcons(string sourceName, List<SpkPackage> packages);
    string GetIconFileName(string sourceName, string packageName);
    string GetIconFileNameWithCacheFolder(string sourceName, string packageName);
    CacheSpkResponseDTO GetSpkResponseFromCache(string sourceName, string model, string version, bool isBeta);
    bool SaveSpkResult(string sourceName, string model, string version, bool isBeta, SpkResult spkResult);

  }
}