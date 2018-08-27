using System.Collections.Generic;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public interface ICacheService: IDomainService
  {   
    void ProcessIconsAsync(string sourceName, List<SpkPackage> packages );
    string GetFileName(string sourceName, string packageName);
    string GetFileNameWithCacheFolder(string sourceName, string packageName);
  }
}