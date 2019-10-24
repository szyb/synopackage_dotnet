using System;
using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Model
{
  public class DownloadFactory : IDownloadFactory
  {
    DownloadServiceImplementation defaultDownloadService;
    private Func<DownloadServiceImplementation, IDownloadService> factory;

    public DownloadFactory(Func<DownloadServiceImplementation, IDownloadService> factory)
    {
      this.factory = factory;
      DownloadServiceImplementation downloadServiceImplementation = DownloadServiceImplementation.RestSharp;

      if (Enum.TryParse<DownloadServiceImplementation>(AppSettingsProvider.AppSettings.DownloadService, true, out var libraryFromSettings))
        downloadServiceImplementation = libraryFromSettings;

      this.defaultDownloadService = downloadServiceImplementation;
    }

    public IDownloadService GetDefaultDownloadService()
    {
      return factory(defaultDownloadService);
    }

    public IDownloadService GetDownloadService(DownloadServiceImplementation donwloadServiceImplementation)
    {
      return factory(donwloadServiceImplementation);
    }

    public IDownloadService GetDownloadServiceBySourceName(string sourceName)
    {
      //here we can use other library to the specific source
      if (string.IsNullOrWhiteSpace(sourceName))
        return factory(defaultDownloadService);
      else
        return factory(defaultDownloadService);
    }
  }
}
