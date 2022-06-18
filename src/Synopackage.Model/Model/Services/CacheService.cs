using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Synopackage.model;
using Synopackage.Model.DTOs;
using Synopackage.Model.SPK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public class CacheService : ICacheService
  {
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

    public async Task ProcessIcons(string sourceName, List<SpkPackage> packages)
    {
      List<Task> downloadTasks = new List<Task>();
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
                if (ShouldDownloadIcon(sourceName, url))
                {
                  var task = DownloadIconAsync(url, sourceName, package.Name);
                  downloadTasks.Add(task);
                }
              }
              catch (Exception ex)
              {
                logger.LogError(ex, "ProcessIcons - could not download or store icon");
              }
            }
          }
          else if (package.Icon != null && package.Icon.Length > 0 && ShouldStoreIcon(sourceName, package.Name))
          {
            try
            {
              byte[] iconBytes = Convert.FromBase64String(package.Icon);
              await File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, package.Name), iconBytes).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
              logger.LogError(ex, "ProcessIcons - could not convert icon from base 64 or store error");
            }
          }
        }
        await Task.WhenAll(downloadTasks.ToArray()).ConfigureAwait(false);
      }
    }

    private async Task DownloadIconAsync(string url, string sourceName, string packageName)
    {
      try
      {
        IDownloadService downloadService = downloadFactory.GetDefaultDownloadService();
        byte[] iconBytes = null;
        iconBytes = await downloadService.DownloadData(url).ConfigureAwait(false);
        if (IsValidIcon(iconBytes))
        {
          await File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, packageName), iconBytes).ConfigureAwait(false);
        }
        else
        {
          var defaultIconBytes = File.ReadAllBytes("wwwroot/assets/package.png"); //TODO: assets folder should be in appsettings
          await File.WriteAllBytesAsync(GetIconFileNameWithCacheFolder(sourceName, packageName), defaultIconBytes).ConfigureAwait(false);
        }
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "DownloadIconAsync - count not download icon");
      }
    }

    public static bool IsCacheFileExpired(FileInfo fi, int cacheValidTimeInHours)
    {
      if (!fi.Exists)
        return true;
      else
        return (DateTime.Now - fi.LastWriteTime).TotalHours > cacheValidTimeInHours;
    }

    private static bool ShouldDownloadIcon(string sourceName, string url)
    {
      //performance improvement for synologyitalia (downloading one icon is taking too much time and eventually it fails)
      if (sourceName == "synologyitalia" && url != null && url.Contains("piwik"))
        return false;
      else
        return true;
    }

    private static bool IsValidIcon(byte[] iconBytes)
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

    public bool SaveSpkResult(string sourceName, string arch, string model, string version, bool isBeta, SpkResult spkResult)
    {
      if (!AppSettingsProvider.AppSettings.CacheSpkServerResponse)
        return false;

      try
      {
        var fileNameByArch = GetResponseCacheByArchFile(sourceName, arch, version, isBeta);
        var serializedData = JsonConvert.SerializeObject(spkResult);
        Random rnd = new Random();
        var IORetryPolicy = Policy.Handle<Exception>()
          .OrResult<bool>(result => !result)
          .WaitAndRetry(4, retryCount => TimeSpan.FromMilliseconds(50 + rnd.Next(0, 200)));

        IORetryPolicy.Execute(() => WriteToFile(fileNameByArch, serializedData));
        return true;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "SaveSpkResult - could not save SPK response to cache");
        return false;
      }
    }

    private bool WriteToFile(string fileName, string serializedData)
    {
      try
      {
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
          var buf = new UTF8Encoding(false).GetBytes(serializedData);
          fileStream.Write(buf);
        }
        return true;
      }
      catch (Exception ex)
      {
        logger.LogError(ex, $"WriteToFile failed: {fileName} - {ex.Message}");
        FileInfo fi = new FileInfo(fileName);
        if (fi.Exists && fi.Length == 0)
        {
          try
          {
            File.Delete(fileName);
          }
          catch (Exception ex2)
          {
            logger.LogError(ex2, $"WriteToFile: unable to delete empty file {fileName} - {ex.Message}");
          }
        }
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

    public async Task<CacheSpkResponseDTO> GetSpkResponseFromCache(string sourceName, string arch, string model, string version, bool isBeta)
    {
      if (!AppSettingsProvider.AppSettings.CacheSpkServerResponse || !AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.HasValue)
      {
        //cache is disabled
        return new CacheSpkResponseDTO() { HasValidCache = false };
      }
      var fileNameByArch = GetResponseCacheByArchFile(sourceName, arch, version, isBeta);

      FileInfo cacheFileInfo = new FileInfo(fileNameByArch);
      if (cacheFileInfo.Exists)
      {
        var expirationInHours = AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHours.Value;
        //a temporary hack for filebot to minimize number of requests to filebot server
        if (sourceName == "filebot")
          expirationInHours = 24;
        var isExpired = IsCacheFileExpired(cacheFileInfo, expirationInHours);
        return new CacheSpkResponseDTO()
        {
          HasValidCache = !isExpired,
          Cache = await CacheService.GetCacheByFile(cacheFileInfo)
        };
      }
      else
      {
        return new CacheSpkResponseDTO()
        {
          HasValidCache = false
        };
      }

    }

    public async Task<CacheSpkResponseDTO> GetSpkResponseForRepositoryFromCache(string sourceName, string arch, string version, bool isBeta)
    {
      var fileNameByArch = GetResponseCacheByArchFile(sourceName, arch, version, isBeta);
      FileInfo cacheFileInfo = new FileInfo(fileNameByArch);
      var expirationInHours = AppSettingsProvider.AppSettings.CacheSpkServerResponseTimeInHoursForRepository;
      //a temporary hack for filebot to minimize number of requests to filebot server
      if (sourceName == "filebot")
        expirationInHours = 24;
      if (!IsCacheFileExpired(cacheFileInfo, expirationInHours))
      {
        try
        {
          CacheSpkResponseDTO result = new CacheSpkResponseDTO()
          {
            HasValidCache = true,
            Cache = await CacheService.GetCacheByFile(cacheFileInfo)
          };
          return result;
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "GetSpkResponseForRepositoryFromCache - could not get SPK response from cache");

        }
      }
      return new CacheSpkResponseDTO() { HasValidCache = false, Cache = null };
    }

    internal static async Task<CacheSpkDTO> GetCacheByFile(FileInfo fileInfo)
    {
      TimeSpan ts = DateTime.Now - fileInfo.LastWriteTime;
      if (fileInfo.Exists)
      {
        var content = await File.ReadAllTextAsync(fileInfo.FullName);
        var deserializedData = JsonConvert.DeserializeObject<SpkResult>(content);
        var result = new CacheSpkDTO()
        {
          SpkResult = deserializedData,
          CacheDate = fileInfo.LastAccessTime,
          CacheOld = ts.TotalSeconds
        };
        return result;
      }
      return null;
    }

    private string GetResponseCacheByArchFile(string sourceName, string arch, string version, bool isBeta)
    {
      var channelString = isBeta ? "beta" : "stable";
      //a temporary hack for filebot to minimize number of requests to filebot server
      if (sourceName == "filebot")
      {
        arch = "allCPUs";
        version = "allVersions";
        channelString = "stable";
      }
      return Path.Combine(AppSettingsProvider.AppSettings.BackendCacheFolder, Utils.CleanFileName($"{sourceName}_{arch}_{version}_{channelString}.{defaultCacheExtension}"));
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
    private static string GetValidUrl(string urlCandidate, bool useSsl = true)
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
