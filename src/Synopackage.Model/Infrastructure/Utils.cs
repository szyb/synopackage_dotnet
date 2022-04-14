using Newtonsoft.Json;
using Synopackage.Model.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Synopackage
{
  public static class Utils
  {
    public static string CleanFileName(string fileName)
    {
      return Path
        .GetInvalidFileNameChars()
        .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_"));
    }

    public static string GetUrlParameters(Dictionary<string, string> parameters)
    {
      if (parameters == null)
        return string.Empty;
      StringBuilder sb = new StringBuilder();
      foreach (var key in parameters.Keys)
      {
        sb.AppendFormat("{0}={1}&", key, parameters[key]);
      }
      string result = sb.ToString();
      if (result.EndsWith("&"))
        result = result[0..^1];
      return result;
    }

    public static string GetSearchLogEntryString(SearchLogEntryDTO logEntry)
    {
      var serializedData = JsonConvert.SerializeObject(logEntry, Formatting.None);
      return serializedData;
    }
  }
}
