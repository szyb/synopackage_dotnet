using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public class CacheService : ICacheService
  {
    IDownloadService downloadService;
    private readonly string defaultIconExtension = "png";
    private readonly string defaultCacheExtension = "cache";

    public CacheService(IDownloadService downloadService)
    {
      if (!Directory.Exists(AppSettingsProvider.AppSettings.FrontendCacheFolder))
        Directory.CreateDirectory(AppSettingsProvider.AppSettings.FrontendCacheFolder);
      if (!Directory.Exists(AppSettingsProvider.AppSettings.BackendCacheFolder))
        Directory.CreateDirectory(AppSettingsProvider.AppSettings.BackendCacheFolder);
      this.downloadService = downloadService;
    }

    public void ProcessIconsAsync(string sourceName, List<SpkPackage> packages)
    {
      throw new NotImplementedException();
      // BackgroundTaskQueue queue = new BackgroundTaskQueue();
      // WebClient client = new WebClient();

      // foreach (var package in packages)
      // {
      //   queue.QueueBackgroundWorkItem(async token =>
      //   {

      //     var data = await client.DownloadDataTaskAsync(new Uri(package.Thumbnail[0]));
      //     var fileName = GetIconFileNameWithCacheFolder(sourceName, package.Name);
      //     SaveIcon(fileName, data);
      //   }
      //   );
      // }
    }

    public void ProcessIcons(string sourceName, List<SpkPackage> packages)
    {
      if (packages != null)
      {
        foreach (var package in packages)
        {
          if (package.Thumbnail != null && package.Thumbnail.Count > 0)
          {
            if (ShouldStoreIcon(sourceName, package.Name))
            {
              try
              {
                var url = GetValidUrl(package.Thumbnail[0]);
                var extension = Path.GetExtension(url);
                var iconBytes = downloadService.DownloadData(url);

                File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, package.Name), iconBytes);
              }
              catch (Exception ex)
              {
                //TODO: log & handle error
              }
            }
          }
          else if (package.Icon != null && package.Icon.Length > 0)
          {
            if (ShouldStoreIcon(sourceName, package.Name))
            {
              try
              {
                byte[] iconBytes = Convert.FromBase64String(package.Icon);
                File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, package.Name), iconBytes);
              }
              catch (Exception ex)
              {
                //TODO: log & handle error
              }
            }
          }
        }
      }
    }

    public bool SaveSpkResult(string sourceName, string model, string version, bool isBeta, SpkResult spkResult)
    {
      if (!AppSettingsProvider.AppSettings.CacheSpkServerResponse)
        return false;

      try
      {
        var fileName = GetResponseCacheFile(sourceName, model, version, isBeta);
        var serializedData = JsonConvert.SerializeObject(spkResult);
        File.WriteAllText(fileName, serializedData);
        return true;
      }
      catch (Exception ex)
      {
        //TODO: log error
        return false;
      }
    }


    public string GetIconFileName(string sourceName, string packageName)
    {
      return Utils.CleanFileName($"{sourceName}_{packageName}.{defaultIconExtension}");
    }

    public string GetIconFileNameWithCacheFolder(string sourceName, string packageName)
    {
      return Path.Combine(AppSettingsProvider.AppSettings.FrontendCacheFolder, GetIconFileName(sourceName, packageName));
    }

    public CacheSpkResponseDTO GetSpkResponseFromCache(string sourceName, string model, string version, bool isBeta)
    {
      CacheSpkResponseDTO res = new CacheSpkResponseDTO();
      var fileName = GetResponseCacheFile(sourceName, model, version, isBeta);
      FileInfo fi = new FileInfo(fileName);
      if (!fi.Exists || !AppSettingsProvider.AppSettings.CacheSpkServerResponse || !AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.HasValue)
      {
        res.Result = false;
        return res;
      }
      TimeSpan ts = DateTime.Now - fi.LastWriteTime;
      if (ts.TotalHours <= AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.Value)
      {
        try
        {
          var content = File.ReadAllText(fileName);
          var deserializedData = JsonConvert.DeserializeObject<SpkResult>(content);
          res.Result = true;
          res.SpkResult = deserializedData;
          return res;
        }
        catch (Exception ex)
        {
          //TODO: log
          res.Result = false;
          return res;
        }
      }
      else
      {
        res.Result = false;
        return res;
      }
    }

    private string GetResponseCacheFile(string sourceName, string model, string version, bool isBeta)
    {
      var channelString = isBeta ? "beta" : "stable";
      return Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, Utils.CleanFileName($"{sourceName}_{model}_{version}_{channelString}.{defaultCacheExtension}"));
    }

    private bool ShouldStoreIcon(string sourceName, string packageName)
    {
      if (!File.Exists(GetIconFileNameWithCacheFolder(sourceName, packageName)))
        return true;
      else
      {
        if (AppSettingsProvider.AppSettings.CacheIconExpirationInDays.HasValue)
        {
          FileInfo fi = new FileInfo(GetIconFileNameWithCacheFolder(sourceName, packageName));
          TimeSpan ts = DateTime.Now - fi.LastWriteTime;
          if (ts.TotalDays <= AppSettingsProvider.AppSettings.CacheIconExpirationInDays.Value)
            return true;
        }
        return false;
      }
    }
    private string GetValidUrl(string urlCandidate, bool useSsl = true)
    {
      string protocol = useSsl ? "https" : "http";
      if (string.IsNullOrWhiteSpace(urlCandidate))
        return null;
      else if (urlCandidate.StartsWith("http", true, CultureInfo.InvariantCulture))
        return urlCandidate;
      else if (urlCandidate.StartsWith("//"))
        return $"{protocol}:{urlCandidate}";
      else
        return $"{protocol}://{urlCandidate}";
    }



  }
}