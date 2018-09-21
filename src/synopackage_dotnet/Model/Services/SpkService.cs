using System.Collections.Generic;
using System.Net;
using ExpressMapper;
using ExpressMapper.Extensions;
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
    public SpkService(ICacheService cacheService, IDownloadService downloadService)
    {
      this.cacheService = cacheService;
      this.downloadService = downloadService;
    }
    public IEnumerable<PackageDTO> GetPackages(string sourceName, string url, string arch, string model, string major, string minor, string build, bool isBeta, string customUserAgent, out string errorMessage)
    {
      errorMessage = null;
      var cacheResult = cacheService.GetSpkResponseFromCache(sourceName, model, build, isBeta);
      SpkResult result = null;
      if (cacheResult.Result == false)
      {
        var request = new RestRequest(Method.POST);
        var unique = $"synology_{arch}_{model}";

        request.AddParameter("language", "enu");
        request.AddParameter("unique", unique);
        request.AddParameter("arch", arch);
        request.AddParameter("major", major);
        request.AddParameter("minor", minor);
        request.AddParameter("build", build);
        request.AddParameter("package_update_channel", isBeta ? "beta" : "stable");
        request.AddParameter("timezone", "Brussels");
        // request.AddHeader("User-Agent", customUserAgent != null ? customUserAgent : unique);
        var userAgent = customUserAgent != null ? customUserAgent : unique;
        var response = downloadService.Execute(url, request, userAgent);

        if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
        {
          var responseContent = response.Content;
          if (responseContent != null)
          {
            System.IO.File.WriteAllText("cache/out.html", responseContent);
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
              cacheService.SaveSpkResult(sourceName, model, build, isBeta, result);
          }
          else
          {
            result = new SpkResult();
          }
        }
        else
        {
          errorMessage = $"{response.StatusCode.ToString()} {response.ErrorMessage}";
          return null;
        }
      }
      else
      {
        result = cacheResult.SpkResult;
      }

      if (result != null)
      {
        this.cacheService.ProcessIcons(sourceName, result.Packages);
        List<PackageDTO> list = new List<PackageDTO>();
        if (result.Packages == null)
          return list;
        foreach (var spkPackage in result.Packages)
        {
          PackageDTO package = new PackageDTO();
          spkPackage.Map(package);
          package.IconFileName = cacheService.GetIconFileName(sourceName, package.Name);
          list.Add(package);
        }
        list.Sort();
        return list;
      }
      else
      {
        errorMessage = "Spk result is empty";
        return null;
      }

    }
  }
}
