using System.Net;
using RestSharp;

namespace synopackage_dotnet.Model.Services
{
  public class DownloadService : IDownloadService
  {

    public DownloadService()
    {

    }    

    private RestClient GetClient(string url)
    {
      var client = new RestClient(url);
      if (!string.IsNullOrEmpty(AppSettingsProvider.AppSettings.ProxyUrl))
      {
          client.Proxy = new WebProxy(AppSettingsProvider.AppSettings.ProxyUrl);
      }
      return client;
    }


    public IRestResponse<T> Execute<T>(string url, RestRequest request) where T : new()
    {
      var client = GetClient(url);
      return client.Execute<T>(request);
    }

    public byte[] DownloadData(string url)
    {
      var client = GetClient(url);
      var request = new RestRequest(Method.GET);
      return client.DownloadData(request);
    }
  }
}