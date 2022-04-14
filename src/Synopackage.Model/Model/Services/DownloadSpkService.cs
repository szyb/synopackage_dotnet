using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Synopackage.Model.DTOs;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public class DownloadSpkService : IDownloadSpkService
  {
    private readonly ISourceService sourceService;
    private readonly ILogger<DownloadSpkService> logger;

    public DownloadSpkService(ISourceService sourceService, ILogger<DownloadSpkService> logger)
    {
      this.sourceService = sourceService;
      this.logger = logger;
    }

    public DownloadResponseDTO DownloadRequest(DownloadRequestDTO downloadRequest, bool isHttps)
    {
      if (!sourceService.ValidateSource(downloadRequest.SourceName))
      {
        logger.LogError($"Coudn't not find source '{downloadRequest.SourceName}'");
        return new DownloadResponseDTO(false, downloadRequest);
      }

      var downloadLink = downloadRequest.RequestUrl;
      string fileName = null;
      DownloadResponseDTO response;
      if (!IsRemoteFileAvailable(downloadRequest.RequestUrl).Result)
      {
        return new DownloadResponseDTO(false, downloadRequest);
      }
      if (IsProxyDownload(downloadRequest, isHttps))
      {
        Guid id = Guid.NewGuid();
        fileName = GetFileNameFromUrl(downloadRequest.RequestUrl);
        downloadLink = $"api/Download/{id}";
        response = new DownloadResponseDTO(true, downloadRequest, downloadLink, fileName);
        var json = JsonConvert.SerializeObject(response);

        Directory.CreateDirectory("DownloadRequests");
        File.WriteAllText(Path.Combine("DownloadRequests", $"{id}.json"), json);
        return response;
      }
      else
      {
        response = new DownloadResponseDTO(true, downloadRequest, downloadLink, null);
      }
      var message = $"Download request: '{downloadRequest.PackageName}' from '{downloadRequest.SourceName}'. Link: {downloadLink}";
      logger.LogInformation(message);
      return response;
    }

    public DownloadResponseDTO GetDownloadDetails(Guid id)
    {
      FileInfo fi = new FileInfo(Path.Combine("DownloadRequests", $"{id}.json"));
      if (fi.Exists)
      {
        var downloadDetailsString = File.ReadAllText(fi.FullName);
        return JsonConvert.DeserializeObject<DownloadResponseDTO>(downloadDetailsString);
      }
      else
        return new DownloadResponseDTO(false, null);
    }

    public string GetFileNameFromContentDisposition(string contentDisposition)
    {
      string fileName = null;
      Regex regex = new Regex($"(filename=\")(?<filename>(.)*)\"");
      var match = regex.Match(contentDisposition);
      if (match.Success)
      {
        fileName = match.Groups["filename"].Value;

      }
      else
        fileName = null;
      return fileName;
    }

    private string GetFileNameFromUrl(string requestUrl)
    {
      Regex regex = new Regex(@$"/(?<filename>(.)*\.spk)");
      var match = regex.Match(requestUrl);
      if (match.Success)
      {
        var fileName = match.Groups["filename"].Value;
        var lastIndex = fileName.LastIndexOf('/');
        return fileName.Substring(lastIndex + 1);
      }
      else
        return null;
    }

    private async Task<bool> IsRemoteFileAvailable(string link)
    {
      HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(link);
      request.Method = "HEAD";
      request.Headers["User-Agent"] = "Synopackage.com";
      HttpWebResponse response = null;
      try
      {
        response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
        if (response != null && response.StatusCode == HttpStatusCode.OK)
          return true;
        else
        {
          logger.LogWarning($"Link is unavailable. Status: {response?.StatusCode}");
          return false;
        }
      }
      catch (WebException wex)
      {
        var response1 = wex.Response as HttpWebResponse;
        if (response1 != null && response1.StatusCode == HttpStatusCode.Forbidden)
        {
          //the resource is available, but for some reason HEAD request is forbidden
          return true;
        }
        else
        {
          logger.LogWarning(wex, $"Link is unavailable {link}");
          return false;
        }
      }
      catch (Exception ex)
      {
        logger.LogWarning(ex, $"Link is unavailable {link}");
        return false;
      }
      finally
      {
        if (response != null)
        {
          response.Close();
          response.Dispose();
        }
      }
    }

    private bool IsProxyDownload(DownloadRequestDTO downloadRequest, bool isHttps)
    {
      if (!isHttps)
        return false;
      if (!AppSettingsProvider.AppSettings.EnableProxyDownloadForInsecureProtocol || !downloadRequest.RequestUrl.StartsWith("http://"))
        return false;
      else
        return true;
    }
  }
}
