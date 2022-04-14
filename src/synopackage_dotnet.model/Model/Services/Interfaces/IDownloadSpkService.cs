using System;
using System.Threading.Tasks;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public interface IDownloadSpkService : IDomainService
  {
    DownloadResponseDTO DownloadRequest(DownloadRequestDTO downloadRequest, bool isHttps);
    DownloadResponseDTO GetDownloadDetails(Guid id);
    string GetFileNameFromContentDisposition(string contentDisposition);
  }
}
