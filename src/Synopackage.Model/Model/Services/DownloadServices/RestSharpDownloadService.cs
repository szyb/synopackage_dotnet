﻿using Microsoft.Extensions.Logging;
using RestSharp;
using Synopackage.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public class RestSharpDownloadService : DownloadServiceAbstract, IDownloadService
  {
    private readonly ILogger<RestSharpDownloadService> logger;

    public RestSharpDownloadService(ILogger<RestSharpDownloadService> logger)
    {
      this.logger = logger;
    }

    private RestClient GetClient(string url, string userAgent = null, bool isRepository = false)
    {
      var client = new RestClient(url);

      SetupProxy(client);

      if (userAgent != null)
        client.UserAgent = userAgent;
      if (isRepository)
        client.Timeout = AppSettingsProvider.AppSettings.DownloadTimeoutInSecondsForRepository * 1000;
      else
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

    public async Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null, bool useGetMethod = false, bool isRepository = false)
    {
      try
      {
        var finalUrl = GetLegacySupportUrl(url, parameters);
        var client = GetClient(finalUrl, userAgent, isRepository);

        IRestRequest request;
        if (!useGetMethod)
          request = new RestRequest(Method.POST);
        else
          request = new RestRequest(Method.GET);

        foreach (var p in parameters)
        {
          request.AddParameter(p.Key, p.Value);
        }
        Stopwatch stopwatch = Stopwatch.StartNew();
        IRestResponse response = await Task.Run(() => client.Execute(request));
        stopwatch.Stop();
        logger.LogInformation("Restsharp executed in {0}ms", stopwatch.ElapsedMilliseconds);
        if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
          return new ExecuteResponse() { Success = true, Content = response.Content };
        else
        {
          if (response.ErrorMessage != null && response.ErrorMessage.Contains("time"))
          {
            logger.LogInformation($"Timeout after {client.Timeout}");
          }
          var errorMessage = $"{response.StatusDescription}; {response.ErrorMessage}; content: {response.Content}";
          logger.LogError($"Error getting response for url: {url} - {errorMessage}");
          return new ExecuteResponse() { Success = false, ErrorMessage = errorMessage };
        }

      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Execute - could not execute request");
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
        var client = GetClient(url, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.36");
        var request = new RestRequest(Method.GET);
        return await Task.Run(() => client.DownloadData(request)); //TODO: use real async method here
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "DownloadData - Could not download data");
        throw;
      }
    }
  }
}
