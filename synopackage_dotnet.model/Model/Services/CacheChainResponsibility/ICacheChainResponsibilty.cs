using System.IO;
using System.Threading.Tasks;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public interface ICacheChainResponsibility
  {
    Task<CacheSpkResponseDTO> Handle(FileInfo firstCacheFile, FileInfo secondCacheFile);
    ICacheChainResponsibility SetupNext(ICacheChainResponsibility nextProcessor);
  }
}
