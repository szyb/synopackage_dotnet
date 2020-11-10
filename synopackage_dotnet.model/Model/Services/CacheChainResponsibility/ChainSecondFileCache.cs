using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class ChainSecondFileCache : CacheChainResponsibilityAbstract, ICacheChainResponsibility
  {
    public ChainSecondFileCache(ILogger logger) : base(logger)
    {
    }

    private bool CanHandle(FileInfo secondCacheFile)
    {
      return secondCacheFile.Exists;
    }

    public override async Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile)
    {
      if (CanHandle(secondCacheFile))
      {
        CacheSpkResponseDTO res = new CacheSpkResponseDTO()
        {
          HasValidCache = false,
          Cache = await GetCacheByFile(secondCacheFile)
        };
        return res;
      }
      return await base.Handle(firstCacheFile, secondCacheFile);
    }

  }
}
