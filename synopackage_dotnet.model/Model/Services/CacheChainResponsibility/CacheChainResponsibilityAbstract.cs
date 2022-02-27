using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;
using System;
using System.IO;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
{
  public abstract class CacheChainResponsibilityAbstract : ICacheChainResponsibility
  {
    protected ICacheChainResponsibility nextProcessor;
    protected readonly ILogger logger;

    protected CacheChainResponsibilityAbstract(ILogger logger)
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
        return new CacheSpkResponseDTO()
        {
          HasValidCache = false
        };
      }
    }

    public ICacheChainResponsibility SetupNext(ICacheChainResponsibility nextProcessor)
    {
      this.nextProcessor = nextProcessor;
      return nextProcessor;
    }

  }
}
