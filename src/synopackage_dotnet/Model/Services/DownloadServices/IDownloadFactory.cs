using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Model
{
  public interface IDownloadFactory
  {
    IDownloadService GetDefaultDownloadService();
    IDownloadService GetDownloadService(DownloadServiceImplementation library);
    IDownloadService GetDownloadServiceBySourceName(string sourceName);
  }
}
