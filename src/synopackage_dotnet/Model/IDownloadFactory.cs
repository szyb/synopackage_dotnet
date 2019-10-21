using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Model
{
  public interface IDownloadFactory
  {
    IDownloadService GetDownloadService(DownloadLibrary library);
    IDownloadService GetDownloadServiceBySourceName(string sourceName);
  }
}