using System.Net;
using RestSharp;

namespace synopackage_dotnet.Model.Services
{
  public class DownloadService : IDownloadService
  {

    public DownloadService()
    {

    }

    private RestClient GetClient(string url, string userAgent = null)
    {
      var client = new RestClient(url);
      if (!string.IsNullOrEmpty(AppSettingsProvider.AppSettings.ProxyUrl))
      {
        client.Proxy = new WebProxy(AppSettingsProvider.AppSettings.ProxyUrl);
      }
      if (userAgent != null)
        client.UserAgent = userAgent;
      client.Timeout = AppSettingsProvider.AppSettings.DownloadTimeoutInSeconds * 1000;
      return client;
    }

    public IRestResponse Execute(string url, RestRequest request, string userAgent = null)
    {
      var client = GetClient(url, userAgent);

      return client.Execute(request);
    }

    public IRestResponse<T> Execute<T>(string url, RestRequest request, string userAgent = null) where T : new()
    {
      var client = GetClient(url, userAgent);
      return client.Execute<T>(request);
    }

    public byte[] DownloadData(string url)
    {
      //act as a regular browser
      var client = GetClient(url, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36 OPR/56.0.3051.36");
      var request = new RestRequest(Method.GET);
      return client.DownloadData(request);
    }
  }
}