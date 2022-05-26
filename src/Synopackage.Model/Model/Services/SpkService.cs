using ExpressMapper.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Synopackage.model.Model.DTOs;
using Synopackage.Model.DTOs;
using Synopackage.Model.Enums;
using Synopackage.Model.SPK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Synopackage.Model.Services
{
  public class SpkService : ISpkService
  {
    private readonly ICacheService cacheService;
    private readonly IDownloadFactory downloadFactory;
    private readonly ILogger<SpkService> logger;

    public SpkService(ICacheService cacheService, IDownloadFactory downloadFactory, ILogger<SpkService> logger)
    {
      this.cacheService = cacheService;
      this.downloadFactory = downloadFactory;
      this.logger = logger;
    }

    public async Task<SourceServerResponseDTO> GetPackages(
      string sourceName,
      string url,
      string arch,
      string model,
      VersionDTO versionDto,
      bool isBeta,
      string customUserAgent,
      bool isSearch,
      string keyword = null,
      bool useGetMethod = false,
      string sourceInfo = null,
      bool isDownloadDisabled = false)
    {
      Stopwatch stopwatch = new Stopwatch();

      string errorMessage;
      ResultFrom resultFrom;
      double? cacheOld = null;
      ParametersDTO parameters = new ParametersDTO(sourceName, model, versionDto, isBeta, keyword);
      SearchLogEntryDTO logEntry = new SearchLogEntryDTO(parameters);
      logEntry.RequestType = isSearch ? RequestType.Search : RequestType.Browse;
      logEntry.LogType = LogType.Parameters;
      logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
      logEntry.LogType = LogType.Result;
      stopwatch.Start();
      var cacheResult = await cacheService.GetSpkResponseFromCache(sourceName, arch, model, versionDto.Build.ToString(), isBeta);
      SpkResult result;
      if (!cacheResult.HasValidCache)
      {
        string unique = $"synology_{arch}_{model}"; //TODO: DSM provide model without leading "DS" or "RS", so we should do the same some day.
        var parametersRequest = PrepareParametersForRequest(arch, unique, versionDto, isBeta, customUserAgent, out var userAgent);

        IDownloadService downloadService = downloadFactory.GetDefaultDownloadService();
        var response = await downloadService.Execute(url, parametersRequest, userAgent, useGetMethod);

        if (response.Success)
        {
          try
          {
            result = ParseResponse(sourceName, url, arch, model, versionDto, isBeta, response.Content);
            resultFrom = ResultFrom.Server;
          }
          catch (Exception ex)
          {
            logger.LogError(ex, $"Unable to parse response from {url}: {response.Content}");
            if (cacheResult.Cache != null)
            {
              logger.LogInformation("Returning data from cache");
              result = cacheResult.Cache.SpkResult;
              resultFrom = ResultFrom.ExpiredCache;
              cacheOld = cacheResult.Cache.CacheOld;
            }
            else
            {
              logger.LogError($"Error getting response for url: {url}. No cache available.");
              return new SourceServerResponseDTO(false, "No data from source server. No cache available", parameters, null, ResultFrom.NotSpecified, null, sourceInfo, isDownloadDisabled);
            }
          }

        }
        else if (cacheResult.Cache != null)
        {
          logger.LogError($"Error getting response for url: {url}: {response.ErrorMessage}");
          result = cacheResult.Cache.SpkResult;
          resultFrom = ResultFrom.ExpiredCache;
          cacheOld = cacheResult.Cache.CacheOld;
        }
        else //no cache && no server response
        {
          errorMessage = $"{response.ErrorMessage}";
          logger.LogError($"Error getting response for url: {url}: {errorMessage}");
          return new SourceServerResponseDTO(false, errorMessage, parameters, null, ResultFrom.NotSpecified, null, sourceInfo, isDownloadDisabled);
        }
      }
      else // get data from Valid cache
      {
        result = cacheResult.Cache.SpkResult;
        resultFrom = ResultFrom.Cache;
        cacheOld = cacheResult.Cache.CacheOld;
      }

      if (result != null)
      {
        var finalResult = await GenerateResult(sourceName, keyword, parameters, result, resultFrom, cacheOld, sourceInfo, isDownloadDisabled);

        stopwatch.Stop();
        logEntry.ResultFrom = resultFrom;
        logEntry.CacheOld = cacheOld;
        logEntry.ExecutionTime = stopwatch.ElapsedMilliseconds;
        logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
        return finalResult;
      }
      else
      {
        errorMessage = "Spk result is empty";
        stopwatch.Stop();
        logEntry.ResultFrom = ResultFrom.NotSpecified;
        logEntry.CacheOld = null;
        logEntry.ExecutionTime = stopwatch.ElapsedMilliseconds;
        logger.LogWarning("Spk result is empty {0}", Utils.GetSearchLogEntryString(logEntry));
        return new SourceServerResponseDTO(false, errorMessage, parameters, null, resultFrom, cacheOld, sourceInfo, isDownloadDisabled);
      }
    }

    public async Task<RawSpkResultDto> GetRawPackages(
      string sourceName,
      string url,
      string arch,
      string unique,
      VersionDTO versionDto,
      bool isBeta,
      string customUserAgent,
      bool isSearch,
      string keyword = null,
      bool useGetMethod = false
      )
    {
      RawSpkResultDto result;
      ParametersDTO parameters = new ParametersDTO(sourceName, unique, versionDto, isBeta, keyword);
      SearchLogEntryDTO logEntry = new SearchLogEntryDTO(parameters);
      logEntry.RequestType = isSearch ? RequestType.Search : RequestType.Browse;
      logEntry.LogType = LogType.Parameters;
      logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
      logEntry.LogType = LogType.Result;
      var stopwatch = Stopwatch.StartNew();
      var cacheResult = await cacheService.GetSpkResponseForRepositoryFromCache(sourceName, arch, versionDto.Build.ToString(), isBeta);
      if (!cacheResult.HasValidCache)
      {
        var parametersRequest = PrepareParametersForRequest(arch, unique, versionDto, isBeta, customUserAgent, out var userAgent);

        IDownloadService downloadService = downloadFactory.GetDefaultDownloadService();
        var response = await downloadService.Execute(url, parametersRequest, userAgent, useGetMethod);

        if (response.Success)
        {
          logEntry.ResultFrom = ResultFrom.Server;
          try
          {
            var rawResponse = ParseResponse(sourceName, url, arch, unique, versionDto, isBeta, response.Content);
            result = new RawSpkResultDto(rawResponse, null);
          }
          catch (Exception ex)
          {
            logger.LogError(ex, $"Could not parse response from server {url}");
            result = new RawSpkResultDto(null, ex.Message);
          }
        }
        else
        {
          logger.LogError($"Could not get any data from the server {url}");
          result = new RawSpkResultDto(null, $"Could not get any data from the server {url}");
        }
      }
      else //return response from valid cache
      {
        result = new RawSpkResultDto(cacheResult.Cache?.SpkResult, null);
        logEntry.CacheOld = cacheResult.Cache.CacheOld;
        logEntry.LogType = LogType.Result;
        logEntry.ResultFrom = ResultFrom.Cache;
      }

      stopwatch.Stop();
      logEntry.ExecutionTime = stopwatch.ElapsedMilliseconds;
      logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
      return result;
    }

    private async Task<SourceServerResponseDTO> GenerateResult(string sourceName, string keyword, ParametersDTO parameters, SpkResult result, ResultFrom resultFrom, double? cacheOld, string sourceInfo = null, bool isDownloadDisabled = false)
    {
      var processIconsTask = this.cacheService.ProcessIcons(sourceName, result.Packages);
      List<PackageDTO> list = new List<PackageDTO>();
      if (result.Packages == null)
      {
        return new SourceServerResponseDTO(true, null, parameters, null, resultFrom, cacheOld);
      }
      foreach (var spkPackage in result.Packages)
      {
        if (string.IsNullOrWhiteSpace(keyword) || KeywordExists(keyword, spkPackage))
        {
          PackageDTO package = new PackageDTO();
          spkPackage.Map(package);
          package.IconFileName = cacheService.GetIconFileName(sourceName, package.Name);
          package.SourceName = sourceName;
          list.Add(package);
        }
      }
      list.Sort();
      await processIconsTask;
      return new SourceServerResponseDTO(true, null, parameters, list, resultFrom, cacheOld, sourceInfo, isDownloadDisabled);
    }

    private SpkResult ParseResponse(string sourceName, string url, string arch, string modelOrUnique, VersionDTO versionDto, bool isBeta, string responseContent)
    {
      SpkResult result;
      if (responseContent != null)
      {
        if (responseContent.Contains("\"packages\""))
        {
          result = JsonConvert.DeserializeObject<SpkResult>(responseContent, new CustomBooleanJsonConverter());
        }
        else
        {
          result = new SpkResult();
          result.Packages = JsonConvert.DeserializeObject<List<SpkPackage>>(responseContent, new CustomBooleanJsonConverter());
        }
        if (result != null)
          cacheService.SaveSpkResult(sourceName, arch, modelOrUnique, versionDto.Build.ToString(), isBeta, result);
      }
      else
      {
        logger.LogWarning($"No data for url: {url}");
        result = new SpkResult();
      }

      return result;
    }


    private static IEnumerable<KeyValuePair<string, object>> PrepareParametersForRequest(string arch, string unique, VersionDTO versionDto, bool isBeta, string customUserAgent, out string userAgent)
    {
      List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();

      list.Add(new KeyValuePair<string, object>("language", "enu"));
      list.Add(new KeyValuePair<string, object>("unique", unique));
      list.Add(new KeyValuePair<string, object>("arch", arch));
      list.Add(new KeyValuePair<string, object>("major", versionDto.Major.ToString()));
      list.Add(new KeyValuePair<string, object>("minor", versionDto.Minor.ToString()));
      list.Add(new KeyValuePair<string, object>("build", versionDto.Build.ToString()));
      list.Add(new KeyValuePair<string, object>("package_update_channel", isBeta ? "beta" : "stable"));
      list.Add(new KeyValuePair<string, object>("timezone", "Brussels"));


      userAgent = customUserAgent ?? unique;
      return list;
    }

    private static bool KeywordExists(string keyword, SpkPackage spkPackage)
    {
      if (string.IsNullOrWhiteSpace(keyword))
        return true;
      if (spkPackage == null)
        return false;
      if (spkPackage.Name != null && spkPackage.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
        return true;
      if (spkPackage.Dname != null && spkPackage.Dname.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
        return true;
      if (spkPackage.Desc != null && spkPackage.Desc.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
        return true;
      return false;
    }
  }
}
