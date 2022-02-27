using System.Collections.Generic;
using System.Threading.Tasks;
using synopackage_dotnet.model.Model.DTOs;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public interface ISpkService : IDomainService
  {
    Task<SourceServerResponseDTO> GetPackages(
      string sourceName,
      string url,
      string arch,
      string model,
      VersionDTO versionDto,
      bool isBeta,
      string customUserAgent,
      bool isSearch,
      string keyword = null,
      bool useGetMethod = false);

    Task<RawSpkResultDto> GetRawPackages(
     string sourceName,
     string url,
     string arch,
     string unique,
     VersionDTO versionDto,
     bool isBeta,
     string customUserAgent,
     bool isSearch,
     string keyword = null,
     bool useGetMethod = false
     );
  }
}
