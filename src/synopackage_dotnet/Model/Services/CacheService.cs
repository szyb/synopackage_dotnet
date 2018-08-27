using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public class CacheService : ICacheService
  {
    public CacheService()
    {
      if (!Directory.Exists(AppSettingsProvider.AppSettings.CacheFolder))
        Directory.CreateDirectory(AppSettingsProvider.AppSettings.CacheFolder);
    }
    public void ProcessIconsAsync(string sourceName, List<SpkPackage> packages)
    {
      BackgroundTaskQueue queue = new BackgroundTaskQueue();
      WebClient client = new WebClient();
      
      foreach (var package in packages)
      {
        queue.QueueBackgroundWorkItem(async token =>
        {
          
          var data = await client.DownloadDataTaskAsync(new Uri(package.Thumbnail[0]));
          var fileName = GetFileNameWithCacheFolder(sourceName, package.Dname);
          SaveIcon(fileName, data);
        }
        );
      }      
    }

    public string GetFileName(string sourceName, string packageName)
    {
      return Utils.CleanFileName($"{sourceName}_{packageName}.png");
    }

    public string GetFileNameWithCacheFolder(string sourceName, string packageName)
    {
      return Path.Combine(AppSettingsProvider.AppSettings.CacheFolder, GetFileName(sourceName, packageName));
    }

    private void SaveIcon(string fileName, byte[] data)
    {

      File.WriteAllBytes(fileName, data);
    }

  }
}