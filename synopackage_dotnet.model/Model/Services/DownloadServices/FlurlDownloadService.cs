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
using System.Text;

namespace synopackage_dotnet.Model.Services
{
  public class FlurlDownloadService : DownloadServiceAbstract, IDownloadService
  {
    ILogger<FlurlDownloadService> logger;

    public HttpCall ErrorHandler { get; private set; }

    public FlurlDownloadService(ILogger<FlurlDownloadService> logger)
    {
      this.logger = logger;
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private IFlurlClient GetClient(string url, string userAgent = null)
    {
      IFlurlClient client = new FlurlClient(url)
        .Configure(settings =>
        {
          settings.HttpClientFactory = new ProxyHttpClientFactory(Environment.GetEnvironmentVariable("http_proxy"));
          settings.Timeout = new TimeSpan(0, 0, AppSettingsProvider.AppSettings.DownloadTimeoutInSeconds);
          settings.OnError = httpCall =>
          {
            httpCall.ExceptionHandled = true;
            throw new Exception(GetExceptionMessageWithoutCall(httpCall.Exception, url), httpCall.Exception);
          };
        });
      if (!string.IsNullOrWhiteSpace(userAgent))
        client.WithHeader("User-Agent", userAgent);

      return client;
    }

    private string GetExceptionMessageWithoutCall(Exception exception, string url)
    {
      if (exception.Message.Contains(url, StringComparison.InvariantCultureIgnoreCase))
      {
        string message = exception.Message;
        int urlIndex = message.IndexOf(url, 0, StringComparison.InvariantCultureIgnoreCase);
        int lastIndexOfColon = message.Substring(0, urlIndex).LastIndexOf(":");
        if (lastIndexOfColon != -1)
          return message.Substring(0, lastIndexOfColon);
        else
          return message.Substring(0, urlIndex);
      }
      else
        return exception.Message;
    }

    public async Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null)
    {
      try
      {
        url = GetLegacySupportUrl(url, parameters);
        IFlurlClient client = GetClient(url, userAgent);

        client.WithHeader("Content-Type", "application/x-www-form-urlencoded");

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
            Content = $"Status code: {(int)response.StatusCode} ({response.StatusCode.ToString()})"
          };
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Flurl error");
        return new ExecuteResponse()
        {
          Success = false,
          ErrorMessage = ex.Message
        };
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
