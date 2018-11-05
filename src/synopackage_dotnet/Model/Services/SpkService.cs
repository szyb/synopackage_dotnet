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

namespace synopackage_dotnet.Model.Services
{
  public class SpkService : ISpkService
  {
    private ICacheService cacheService;
    private IDownloadService downloadService;
    private ILogger<SpkService> logger;

    public SpkService(ICacheService cacheService, IDownloadService downloadService, ILogger<SpkService> logger)
    {
      this.cacheService = cacheService;
      this.downloadService = downloadService;
      this.logger = logger;
    }

    public SourceServerResponseDTO GetPackages(string sourceName, string url, string arch, string model, VersionDTO versionDto, bool isBeta, string customUserAgent, string keyword = null)
    {
      string errorMessage = null;
      ParametersDTO parameters = new ParametersDTO(sourceName, model, versionDto, isBeta, keyword);
      var cacheResult = cacheService.GetSpkResponseFromCache(sourceName, model, versionDto.Build.ToString(), isBeta);
      SpkResult result = null;
      if (cacheResult.Result == false)
      {
        string finalUrl;
        string userAgent;
        RestRequest request = PrepareRequest(url, arch, model, versionDto, isBeta, customUserAgent, out userAgent, out finalUrl);

        var response = downloadService.Execute(finalUrl, request, userAgent);

        if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
        {
          result = ParseResponse(sourceName, url, model, versionDto, isBeta, response);
        }
        else
        {
          errorMessage = $"{response.StatusDescription} {response.ErrorMessage}";
          logger.LogError($"Error getting response for url: {url}: {errorMessage}");
          return new SourceServerResponseDTO(false, errorMessage, parameters, null);
        }
      }
      else
      {
        result = cacheResult.SpkResult;
      }

      if (result != null)
      {
        return GenerateResult(sourceName, keyword, parameters, result);
      }
      else
      {
        errorMessage = "Spk result is empty";
        return new SourceServerResponseDTO(false, errorMessage, parameters, null);
      }
    }

    private SourceServerResponseDTO GenerateResult(string sourceName, string keyword, ParametersDTO parameters, SpkResult result)
    {
      this.cacheService.ProcessIcons(sourceName, result.Packages);
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
          list.Add(package);
        }
      }
      list.Sort();
      return new SourceServerResponseDTO(true, null, parameters, list);
    }

    private SpkResult ParseResponse(string sourceName, string url, string model, VersionDTO versionDto, bool isBeta, IRestResponse response)
    {
      SpkResult result;
      var responseContent = response.Content;
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

    private RestRequest PrepareRequest(string url, string arch, string model, VersionDTO versionDto, bool isBeta, string customUserAgent, out string userAgent, out string finalUrl)
    {
      var request = new RestRequest(Method.POST);
      var unique = $"synology_{arch}_{model}";

      request.AddParameter("language", "enu");
      request.AddParameter("unique", unique);
      request.AddParameter("arch", arch);
      request.AddParameter("major", versionDto.Major.ToString());
      request.AddParameter("minor", versionDto.Minor.ToString());
      request.AddParameter("build", versionDto.Build.ToString());
      request.AddParameter("package_update_channel", isBeta ? "beta" : "stable");
      request.AddParameter("timezone", "Brussels");

      finalUrl = GetLegacySupportUrl(url, request);
      userAgent = customUserAgent != null ? customUserAgent : unique;

      return request;
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

    private string GetLegacySupportUrl(string url, RestRequest request)
    {
      Dictionary<string, string> dictParamValue = new Dictionary<string, string>();
      request.Parameters.ForEach(item =>
      {
        dictParamValue.Add(item.Name, item.Value.ToString());
      });
      string urlParams = Utils.GetUrlParameters(dictParamValue);
      if (url.EndsWith("/"))
        return $"{url}?{urlParams}";
      else
        return $"{url}/?{urlParams}";
    }
  }
}
