using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;

namespace synopackage_dotnet.Model.Services
{
  public class CacheService : ICacheService
  {
    // IDownloadService downloadService;
    private readonly ILogger<CacheService> logger;
    private readonly IDownloadFactory downloadFactory;
    private readonly string defaultIconExtension = "png";
    private readonly string defaultCacheExtension = "cache";

    public CacheService(IDownloadFactory factory, ILogger<CacheService> logger)
    {
      if (!Directory.Exists(AppSettingsProvider.AppSettings.FrontendCacheFolder))
        Directory.CreateDirectory(AppSettingsProvider.AppSettings.FrontendCacheFolder);
      if (!Directory.Exists(AppSettingsProvider.AppSettings.BackendCacheFolder))
        Directory.CreateDirectory(AppSettingsProvider.AppSettings.BackendCacheFolder);
      this.downloadFactory = factory;
      this.logger = logger;
    }

    public void ProcessIcons(string sourceName, List<SpkPackage> packages)
    {
      IDownloadService downloadService = downloadFactory.GetDefaultDownloadService();
      if (packages != null)
      {
        byte[] defaultIconBytes = null;
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
                byte[] iconBytes = null;
                if (ShouldDownloadIcon(sourceName, url))
                  iconBytes = Task.Run(() => downloadService.DownloadData(url)).Result;
                if (IsValidIcon(iconBytes))
                {
                  File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, package.Name), iconBytes);
                }
                else
                {
                  if (defaultIconBytes == null)
                    defaultIconBytes = File.ReadAllBytes("wwwroot/assets/package.png"); //TODO: assets folder should be in appsettings
                  File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, package.Name), defaultIconBytes);
                }
              }
              catch (Exception ex)
              {
                logger.LogError(ex, "ProcessIcons - could not download or store icon");
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
                logger.LogError(ex, "ProcessIcons - could not convert icon from base 64 or store error");
              }
            }
          }
        }
      }
    }

    private bool ShouldDownloadIcon(string sourceName, string url)
    {
      //performance improvement for synologyitalia (downloading one icon is taking too much time and eventually it fails)
      if (sourceName == "synologyitalia" && url != null && url.Contains("piwik"))
        return false;
      else
        return true;
    }

    private bool IsValidIcon(byte[] iconBytes)
    {
      if (iconBytes == null)
        return false;
      //PNG
      if (iconBytes.Length >= 8
        && iconBytes[0] == 137
        && iconBytes[1] == 80
        && iconBytes[2] == 78
        && iconBytes[3] == 71
        && iconBytes[4] == 13
        && iconBytes[5] == 10
        && iconBytes[6] == 26
        && iconBytes[7] == 10)
        return true;
      //GIF
      else if (iconBytes.Length >= 3
        && iconBytes[0] == Encoding.ASCII.GetBytes("G")[0]
        && iconBytes[1] == Encoding.ASCII.GetBytes("I")[0]
        && iconBytes[2] == Encoding.ASCII.GetBytes("F")[0]
        )
        return true;
      //JFIF
      else if (iconBytes.Length >= 10
        && iconBytes[6] == Encoding.ASCII.GetBytes("J")[0]
        && iconBytes[7] == Encoding.ASCII.GetBytes("F")[0]
        && iconBytes[8] == Encoding.ASCII.GetBytes("I")[0]
        && iconBytes[9] == Encoding.ASCII.GetBytes("F")[0]
        )
        return true;
      else
      {
        return false;
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
        logger.LogError(ex, "SaveSpkResult - could not save SPK response to cache");
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
          res.CacheDate = fi.LastAccessTime;
          res.CacheOld = ts.TotalSeconds;
          return res;
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "GetSpkResponseFromCache - could not get SPK response from cache");
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
