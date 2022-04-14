using System.Collections.Generic;
using System.Threading.Tasks;
using Synopackage.Model.DTOs;

namespace Synopackage.Model.Services
{
  public interface IDownloadService
  {
    Task<ExecuteResponse> Execute(string url, IEnumerable<KeyValuePair<string, object>> parameters, string userAgent = null, bool useGetMethod = false, bool isRepository = false);
    Task<byte[]> DownloadData(string url);
  }
}
