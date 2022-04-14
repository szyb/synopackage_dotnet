using System.Collections.Generic;
using System.Linq;

namespace Synopackage.Model.Services
{
  public abstract class DownloadServiceAbstract
  {
    protected string GetLegacySupportUrl(string url, IEnumerable<KeyValuePair<string, object>> parameters)
    {
      string urlParams = GetParameters(parameters);
      if (url.EndsWith("/") || url.EndsWith(".json"))
        return $"{url}?{urlParams}";
      else
        return $"{url}/?{urlParams}";
    }

    protected static string GetParameters(IEnumerable<KeyValuePair<string, object>> parameters)
    {
      Dictionary<string, string> dictParamValue = new Dictionary<string, string>();
      parameters.ToList().ForEach(item =>
      {
        dictParamValue.Add(item.Key, item.Value.ToString());
      });
      string urlParams = Utils.GetUrlParameters(dictParamValue);
      return urlParams;
    }
  }
}