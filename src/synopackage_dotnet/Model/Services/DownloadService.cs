using System;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace synopackage_dotnet.Model.Services
{
  public class DownloadService : IDownloadService
  {
    ILogger<DownloadService> logger;

    public DownloadService(ILogger<DownloadService> logger)
    {
      this.logger = logger;
    }

    private RestClient GetClient(string url, string userAgent = null)
    {
      var client = new RestClient(url);

      SetupProxy(client);

      if (userAgent != null)
        client.UserAgent = userAgent;
      client.Timeout = AppSettingsProvider.AppSettings.DownloadTimeoutInSeconds * 1000;
      return client;
    }

    private static void SetupProxy(RestClient client)
    {
      var httpProxy = Environment.GetEnvironmentVariable("http_proxy");
      if (!string.IsNullOrWhiteSpace(httpProxy))
      {
        client.Proxy = new WebProxy(httpProxy);
        client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
      }
    }

    public IRestResponse Execute(string url, RestRequest request, string userAgent = null)
    {
      try
      {
        var client = GetClient(url, userAgent);

        return client.Execute(request);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Execute - could not execute request");
        throw;
      }

    }

    public IRestResponse<T> Execute<T>(string url, RestRequest request, string userAgent = null) where T : new()
    {
      try
      {
        var client = GetClient(url, userAgent);
        return client.Execute<T>(request);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Execute<T> - could not execute request");
        throw;
      }
    }

    public byte[] DownloadData(string url)
    {
      try
      {
        //act as a regular browser
        var client = GetClient(url, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.36");
        var request = new RestRequest(Method.GET);
        return client.DownloadData(request);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "DownloadData - Could not download data");
        throw;
      }
    }
  }
}