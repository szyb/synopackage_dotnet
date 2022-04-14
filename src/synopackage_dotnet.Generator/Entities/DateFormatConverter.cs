using Newtonsoft.Json.Converters;

namespace synopackage_dotnet.Generator.Entities
{
  public class DateFormatConverter : IsoDateTimeConverter
  {
    public DateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }
}
