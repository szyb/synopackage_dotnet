using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using synopackage_dotnet.model;
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
      return !firstCacheFile.IsCacheFileExpired(AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.Value);
    }
    public override async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (CanHandle(firstCacheFile))
      {
        try
        {
          CacheSpkResponseDTO res = new CacheSpkResponseDTO()
          {
            HasValidCache = true,
            Cache = await CacheService.GetCacheByFile(firstCacheFile)
          };
          return res;
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "ChainNotExpiredCache -  could not get SPK response from cache");
          return null;
        }

      }
      else
        return await base.Handle(firstCacheFile, secondCacheFile);
    }
  }
}
