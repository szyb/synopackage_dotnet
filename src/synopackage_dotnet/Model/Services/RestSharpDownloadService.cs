using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class RestSharpDownloadService : DownloadServiceAbstract, IDownloadService
  {
    ILogger<RestSharpDownloadService> logger;

    public RestSharpDownloadService(ILogger<RestSharpDownloadService> logger)
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

    private static void SetupProxy(IRestClient client)
    {
      var httpProxy = Environment.GetEnvironmentVariable("http_proxy");
      if (!string.IsNullOrWhiteSpace(httpProxy))
      {
        client.Proxy = new WebProxy(httpProxy);
        client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
      }
    }

    public async Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null)
    {
      try
      {
        var finalUrl = GetLegacySupportUrl(url, parameters);
        var client = GetClient(finalUrl, userAgent);
        IRestRequest request = new RestRequest(Method.POST);
        foreach (var p in parameters)
        {
          request.AddParameter(p.Key, p.Value);
        }
        IRestResponse response = await Task.Run(() => client.Execute(request));

        if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)

          return new ExecuteResponse() { Success = true, Content = response.Content };
        else
        {
          var errorMessage = $"{response.StatusDescription} {response.ErrorMessage}";
          logger.LogError($"Error getting response for url: {url}: {errorMessage}");
          return new ExecuteResponse() { Success = false, ErrorMessage = errorMessage };
        }

      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Execute - could not execute request");
        throw;
      }
    }

    public async Task<byte[]> DownloadData(string url)
    {
      try
      {
        //act as a regular browser
        var client = GetClient(url, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.36");
        var request = new RestRequest(Method.GET);
        return await Task.Run(() => client.DownloadData(request));
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "DownloadData - Could not download data");
        throw;
      }
    }
  }
}
