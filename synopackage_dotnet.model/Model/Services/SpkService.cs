using System;
using System.Collections.Generic;
using System.Net;
using ExpressMapper;
using ExpressMapper.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using synopackage_dotnet.Model.DTOs;
using synopackage_dotnet.Model.SPK;
using Serilog.Extensions.Logging;
using Serilog.Context;
using synopackage_dotnet.Model.Enums;
using System.Threading.Tasks;

namespace synopackage_dotnet.Model.Services
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
      bool useGetMethod = false)
    {
      ExecutionTime et = new ExecutionTime();

      string errorMessage = null;
      ParametersDTO parameters = new ParametersDTO(sourceName, model, versionDto, isBeta, keyword);
      SearchLogEntryDTO logEntry = new SearchLogEntryDTO(parameters);
      logEntry.RequestType = isSearch ? RequestType.Search : RequestType.Browse;
      logEntry.LogType = LogType.Parameters;
      logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
      logEntry.LogType = LogType.Result;
      et.Start();
      var cacheResult = await cacheService.GetSpkResponseFromCache(sourceName, model, versionDto.Build.ToString(), isBeta);
      SpkResult result = null;
      if (cacheResult.Result == false)
      {
        string userAgent;
        var parametersRequest = PrepareParameters(arch, model, versionDto, isBeta, customUserAgent, out userAgent);

        IDownloadService downloadService = downloadFactory.GetDefaultDownloadService();
        var response = await downloadService.Execute(url, parametersRequest, userAgent, useGetMethod);

        if (response.Success)
        {
          logEntry.ResultFrom = ResultFrom.Server;
          result = ParseResponse(sourceName, url, model, versionDto, isBeta, response.Content);
        }
        else
        {
          errorMessage = $"{response.ErrorMessage}";
          logger.LogError($"Error getting response for url: {url}: {errorMessage}");
          return new SourceServerResponseDTO(false, errorMessage, parameters, null);
        }

      }
      else
      {
        result = cacheResult.SpkResult;
        logEntry.ResultFrom = ResultFrom.Cache;
        logEntry.CacheOld = cacheResult.CacheOld;
      }

      if (result != null)
      {
        var finalResult = await GenerateResult(sourceName, keyword, parameters, result);
        et.Stop();
        logEntry.ExecutionTime = et.GetDiff();
        logger.LogInformation(Utils.GetSearchLogEntryString(logEntry));
        return finalResult;
      }
      else
      {
        errorMessage = "Spk result is empty";
        et.Stop();
        logEntry.ExecutionTime = et.GetDiff();
        logger.LogWarning("Spk result is empty {0}", Utils.GetSearchLogEntryString(logEntry));
        return new SourceServerResponseDTO(false, errorMessage, parameters, null);
      }
    }

    private async Task<SourceServerResponseDTO> GenerateResult(string sourceName, string keyword, ParametersDTO parameters, SpkResult result)
    {
      await this.cacheService.ProcessIcons(sourceName, result.Packages);
      List<PackageDTO> list = new List<PackageDTO>();
      if (result.Packages == null)
      {
        return new SourceServerResponseDTO(true, null, parameters, null);
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
      return new SourceServerResponseDTO(true, null, parameters, list);
    }

    private SpkResult ParseResponse(string sourceName, string url, string model, VersionDTO versionDto, bool isBeta, string responseContent)
    {
      SpkResult result;
      if (responseContent != null)
      {
        responseContent = responseContent.Replace("\\n", "\n");
        if (responseContent.Contains("\"packages\""))
        {
          result = JsonConvert.DeserializeObject<SpkResult>(responseContent);
        }
        else
        {
          result = new SpkResult();
          result.Packages = JsonConvert.DeserializeObject<List<SpkPackage>>(responseContent);
        }
        if (result != null)
          cacheService.SaveSpkResult(sourceName, model, versionDto.Build.ToString(), isBeta, result);
      }
      else
      {
        logger.LogWarning($"No data for url: {url}");
        result = new SpkResult();
      }

      return result;
    }


    private IEnumerable<KeyValuePair<string, object>> PrepareParameters(string arch, string model, VersionDTO versionDto, bool isBeta, string customUserAgent, out string userAgent)
    {
      List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
      var unique = $"synology_{arch}_{model}";

      list.Add(new KeyValuePair<string, object>("language", "enu"));
      list.Add(new KeyValuePair<string, object>("unique", unique));
      list.Add(new KeyValuePair<string, object>("arch", arch));
      list.Add(new KeyValuePair<string, object>("major", versionDto.Major.ToString()));
      list.Add(new KeyValuePair<string, object>("minor", versionDto.Minor.ToString()));
      list.Add(new KeyValuePair<string, object>("build", versionDto.Build.ToString()));
      list.Add(new KeyValuePair<string, object>("package_update_channel", isBeta ? "beta" : "stable"));
      list.Add(new KeyValuePair<string, object>("timezone", "Brussels"));


      userAgent = customUserAgent != null ? customUserAgent : unique;
      return list;
    }

    private bool KeywordExists(string keyword, SpkPackage spkPackage)
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
