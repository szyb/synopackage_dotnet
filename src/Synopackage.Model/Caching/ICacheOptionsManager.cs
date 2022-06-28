using Synopackage.Model.DTOs;

namespace Synopackage.Model.Caching;
public partial interface ICacheOptionsManager
{
  string GetArchStringForCacheFile(string arch, string sourceName = null);
  string GetVersionStringForCacheFile(VersionDTO version, string sourceName = null);
  string GetChannelStringForCacheFile(bool isBeta, string sourceName = null);
}
