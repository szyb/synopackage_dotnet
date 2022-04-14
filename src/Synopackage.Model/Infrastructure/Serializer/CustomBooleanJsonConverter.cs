using Newtonsoft.Json;
using System;
namespace synopackage_dotnet.Model
{
  public class CustomBooleanJsonConverter : JsonConverter<bool>
  {
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      if (reader.ValueType == typeof(string))
      {
        var val = reader.Value as string;
        if ("true".Equals(val, StringComparison.InvariantCultureIgnoreCase))
          return true;
        else if ("false".Equals(val, StringComparison.InvariantCultureIgnoreCase))
          return false;
        else if (val.Length == 1)
          return Convert.ToBoolean(Convert.ToByte(reader.Value));
        else
          throw new Exception($"Unable to parse {val} as boolean");
      }
      else if (reader.ValueType == typeof(bool))
        return (bool)reader.Value;
      else
        throw new Exception($"Unable to parse type {reader.ValueType} to boolean");

    }

    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
    {
      serializer.Serialize(writer, value);
    }
  }
}
