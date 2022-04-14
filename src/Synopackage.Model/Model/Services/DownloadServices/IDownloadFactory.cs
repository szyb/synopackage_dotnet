using Synopackage.Model.Enums;
using Synopackage.Model.Services;

namespace Synopackage.Model
{
  public interface IDownloadFactory
  {
    IDownloadService GetDefaultDownloadService();
    IDownloadService GetDownloadService(DownloadServiceImplementation library);
    IDownloadService GetDownloadServiceBySourceName(string sourceName);
  }
}
