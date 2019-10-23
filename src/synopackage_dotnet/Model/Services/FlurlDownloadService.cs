using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Flurl;
using Flurl.Http;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public class FlurlDownloadService : DownloadServiceAbstract, IDownloadService
  {
    ILogger<FlurlDownloadService> logger;

    public HttpCall ErrorHandler { get; private set; }

    public FlurlDownloadService(ILogger<FlurlDownloadService> logger)
    {
      this.logger = logger;
    }

    private IFlurlClient GetClient(string url, string userAgent = null)
    {
      IFlurlClient client = new FlurlClient(url)
        .Configure(settings =>
        {
          settings.HttpClientFactory = new ProxyHttpClientFactory(Environment.GetEnvironmentVariable("http_proxy"));
          settings.Timeout = new TimeSpan(0, 0, AppSettingsProvider.AppSettings.DownloadTimeoutInSeconds);
        });
      if (!string.IsNullOrWhiteSpace(userAgent))
        client.WithHeader("User-Agent", userAgent);

      return client;
    }

    // private RestClient GetClient(string url, string userAgent = null)
    // {
    //   var client = new RestClient(url);

    //   SetupProxy(client);

    //   if (userAgent != null)
    //     client.UserAgent = userAgent;
    //   client.Timeout = AppSettingsProvider.AppSettings.DownloadTimeoutInSeconds * 1000;
    //   return client;
    // }

    // private static void SetupProxy(RestClient client)
    // {
    //   var httpProxy = Environment.GetEnvironmentVariable("http_proxy");
    //   if (!string.IsNullOrWhiteSpace(httpProxy))
    //   {
    //     client.Proxy = new WebProxy(httpProxy);
    //     client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
    //   }
    // }



    public async Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null)
    {
      try
      {
        IFlurlClient client = GetClient(url);

        client.WithHeader("Content-Type", "application/x-www-form-urlencoded");
        if (!string.IsNullOrWhiteSpace(userAgent))
          client.WithHeader("User-Agent", userAgent);

        var response = await client
          .Request()
          .PostUrlEncodedAsync(GetParameters(parameters));
        if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
          return new ExecuteResponse()
          {
            Success = true,
            Content = response.Content.ReadAsStringAsync().Result
          };
        else
          return new ExecuteResponse()
          {
            Success = false,
            // ErrorMessage = response;
          };
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Flurl error");
        throw;
        // return new ExecuteResponse()
        // {
        //   Success = false,
        //   ErrorMessage = ex.Message
        // };
      }
    }

    public async Task<byte[]> DownloadData(string url)
    {
      try
      {
        //act as a regular browser
        IFlurlClient client = GetClient(url, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.36");
        return await client.Request().GetBytesAsync();
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "DownloadData - Could not download data");
        throw;
      }
    }
  }
}
