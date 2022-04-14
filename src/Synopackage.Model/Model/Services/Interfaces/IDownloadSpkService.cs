using System;
using System.Threading.Tasks;
using Synopackage.Model.DTOs;

namespace Synopackage.Model.Services
{
  public interface IDownloadSpkService : IDomainService
  {
    DownloadResponseDTO DownloadRequest(DownloadRequestDTO downloadRequest, bool isHttps);
    DownloadResponseDTO GetDownloadDetails(Guid id);
    string GetFileNameFromContentDisposition(string contentDisposition);
  }
}
