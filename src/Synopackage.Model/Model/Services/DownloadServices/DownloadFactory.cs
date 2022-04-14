using Synopackage.Model.Enums;
using Synopackage.Model.Services;
using System;

namespace Synopackage.Model
{
  public class DownloadFactory : IDownloadFactory
  {
    private readonly DownloadServiceImplementation defaultDownloadService;
    private readonly Func<DownloadServiceImplementation, IDownloadService> factory;

    public DownloadFactory(Func<DownloadServiceImplementation, IDownloadService> factory)
    {
      this.factory = factory;
      DownloadServiceImplementation downloadServiceImplementation = DownloadServiceImplementation.RestSharp;

      if (Enum.TryParse<DownloadServiceImplementation>(AppSettingsProvider.AppSettings.DownloadService, true, out var libraryFromSettings))
        downloadServiceImplementation = libraryFromSettings;

      this.defaultDownloadService = downloadServiceImplementation;
    }

    public IDownloadService GetDefaultDownloadService() => factory(defaultDownloadService);

    public IDownloadService GetDownloadService(DownloadServiceImplementation library) => factory(library);

    public IDownloadService GetDownloadServiceBySourceName(string sourceName) => factory(defaultDownloadService);
  }
}
