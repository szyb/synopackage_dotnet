using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public abstract class CacheChainResponsibilityAbstract : ICacheChainResponsibility
  {
    protected ICacheChainResponsibility nextProcessor;
    private ILogger logger;

    public CacheChainResponsibilityAbstract(ILogger logger)
    {
      this.logger = logger;
    }
    public virtual async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (this.nextProcessor != null)
      {
        return await this.nextProcessor.Handle(firstCacheFile, secondCacheFile);
      }
      else
      {
        return null;
      }
    }

    public ICacheChainResponsibility SetupNext(ICacheChainResponsibility nextProcessor)
    {
      this.nextProcessor = nextProcessor;
      return nextProcessor;
    }

    protected async Task<CacheSpkDTO> GetCacheByFile(FileInfo fileInfo)
    {
      try
      {
        TimeSpan ts = DateTime.Now - fileInfo.LastWriteTime;
        if (fileInfo.Exists)
        {
          var content = await File.ReadAllTextAsync(fileInfo.FullName);
          var deserializedData = JsonConvert.DeserializeObject<SpkResult>(content);
          var result = new CacheSpkDTO()
          {
            SpkResult = deserializedData,
            CacheDate = fileInfo.LastAccessTime,
            CacheOld = ts.TotalSeconds
          };
          return result;
        }
        return null;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "GetSpkResponseFromCache - could not get SPK response from cache");
        return null;
      }
    }
  }
}
