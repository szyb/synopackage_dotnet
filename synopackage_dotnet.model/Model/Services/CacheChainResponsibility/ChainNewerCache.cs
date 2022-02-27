using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
{
  public class ChainNewerCache : CacheChainResponsibilityAbstract, ICacheChainResponsibility
  {
    public ChainNewerCache(ILogger logger) : base(logger)
    {
    }

    private bool CanHandle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (firstCacheFile.Exists && secondCacheFile.Exists)
        return true;
      else
        return false;
    }

    public override async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (CanHandle(firstCacheFile, secondCacheFile))
      {
        try
        {
          FileInfo newerCacheFile;
          if (firstCacheFile.LastAccessTime >= secondCacheFile.LastAccessTime)
            newerCacheFile = firstCacheFile;
          else
            newerCacheFile = secondCacheFile;
          CacheSpkResponseDTO res = new CacheSpkResponseDTO()
          {
            HasValidCache = false,
            Cache = await CacheService.GetCacheByFile(newerCacheFile)
          };
          return res;
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "ChainNewerCache -  could not get SPK response from cache");
          return null;
        }
      }
      else
        return await base.Handle(firstCacheFile, secondCacheFile);
    }

  }
}
