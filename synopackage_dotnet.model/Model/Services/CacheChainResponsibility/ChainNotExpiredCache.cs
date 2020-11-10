using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class ChainNotExpiredCache : CacheChainResponsibilityAbstract, ICacheChainResponsibility
  {
    public ChainNotExpiredCache(ILogger logger) : base(logger)
    {
    }

    private bool CanHandle(FileInfo firstCacheFile)
    {
      if (firstCacheFile.Exists && (DateTime.Now - firstCacheFile.LastWriteTime).TotalHours <= AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.Value)
        return true;
      else
        return false;
    }
    public override async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (CanHandle(firstCacheFile))
      {
        CacheSpkResponseDTO res = new CacheSpkResponseDTO()
        {
          HasValidCache = true,
          Cache = await GetCacheByFile(firstCacheFile)
        };
        return res;
      }
      else
        return await base.Handle(firstCacheFile, secondCacheFile);
    }
  }
}
