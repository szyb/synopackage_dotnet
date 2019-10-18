using System.Collections.Generic;
using System.Threading.Tasks;
using synopackage_dotnet.Model.DTOs;

namespace synopackage_dotnet.Model.Services
{
  public interface IDownloadService
  {
    Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null);
    Task<byte[]> DownloadData(string url);
  }
}