using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synopackage.model.Model.DTOs;
using Synopackage.Model.DTOs;
using Synopackage.Model.SPK;

namespace Synopackage.Model.Services
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
      bool useGetMethod = false,
      string sourceInfo = null,
      bool isDownloadDisabled = false);

    [Obsolete("This method was split into two: UpdateCacheOnly & GetFromValidCacheOnly")]
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

    Task<(string, bool)> UpdateCacheOnly(
     string sourceName,
     string url,
     string arch,
     string unique,
     VersionDTO versionDto,
     bool isBeta,
     string customUserAgent,
     bool isSearch,
     string keyword = null,
     bool useGetMethod = false);

    Task<RawSpkResultDto> GetFromValidCacheOnly(
      string sourceName,
      string arch,
      VersionDTO versionDto,
      bool isBeta);


  }
}
