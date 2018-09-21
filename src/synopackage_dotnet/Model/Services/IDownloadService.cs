using RestSharp;

namespace synopackage_dotnet.Model.Services
{
  public interface IDownloadService
  {
      IRestResponse<T> Execute<T>(string url, RestRequest request) where T : new();
      byte[] DownloadData(string url);
  }
}