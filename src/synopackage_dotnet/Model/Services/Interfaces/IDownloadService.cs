using RestSharp;

namespace synopackage_dotnet.Model.Services
{
  public interface IDownloadService
  {
    IRestResponse Execute(string url, RestRequest request, string userAgent = null);
    IRestResponse<T> Execute<T>(string url, RestRequest request, string userAgent = null) where T : new();
    byte[] DownloadData(string url);


  }
}