using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class ChainFirstFileCache : CacheChainResponsibilityAbstract, ICacheChainResponsibility
  {
    public ChainFirstFileCache(ILogger logger) : base(logger)
    {
    }

    private bool CanHandle(FileInfo firstCacheFile)
    {
      return firstCacheFile.Exists;
    }

    public override async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (CanHandle(firstCacheFile))
      {
        try
        {
          CacheSpkResponseDTO res = new CacheSpkResponseDTO()
          {
            HasValidCache = false,
            Cache = await CacheService.GetCacheByFile(firstCacheFile)
          };
          return res;
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "ChainFirstFileCache -  could not get SPK response from cache");
          return null;
        }
      }
      return await base.Handle(firstCacheFile, secondCacheFile);
    }

  }
}
