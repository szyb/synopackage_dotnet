using Newtonsoft.Json.Converters;

namespace synopackage_dotnet
{
  public class DateFormatConverter : IsoDateTimeConverter
  {
    public DateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }
}