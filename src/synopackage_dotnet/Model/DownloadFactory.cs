using System;
using synopackage_dotnet.Model.Enums;
using synopackage_dotnet.Model.Services;

namespace synopackage_dotnet.Model
{
  public class DownloadFactory : IDownloadFactory
  {
    DownloadLibrary defaultDownloadService;
    private Func<DownloadLibrary, IDownloadService> factory;

    public DownloadFactory(Func<DownloadLibrary, IDownloadService> factory)
    {
      this.factory = factory;
      this.defaultDownloadService = DownloadLibrary.RestSharp;
    }

    public IDownloadService GetDownloadService(DownloadLibrary library)
    {
      return factory(library);
    }

    public IDownloadService GetDownloadServiceBySourceName(string sourceName)
    {
      if (string.IsNullOrWhiteSpace(sourceName))
        return factory(defaultDownloadService);
      else if (sourceName.Equals("jdel"))
        return factory(DownloadLibrary.Flurl);
      else
        return factory(defaultDownloadService);
    }
  }
}