using System.Collections.Generic;
using synopackage_dotnet.Model.DTOs;
namespace synopackage_dotnet.Model.Services
{
  public interface ISpkService : IDomainService
  {
    SourceServerResponseDTO GetPackages(string sourceName, string url, string arch, string model, VersionDTO versionDto, bool isBeta, string customUserAgent, bool isSearch, string keyword = null);

  }
}
