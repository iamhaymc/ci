using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Trane.Submittals.Pipeline
{
  public static class Json
  {
    public static T FromJson<T>(string json)
    {
      JsonSerializerOptions options = Json.IndentByDefault ? IndentedJsonOptions : Json.UnindentedJsonOptions;
      return JsonSerializer.Deserialize<T>(json, options);
    }

    public static JsonNode FromJson(string json)
    {
      return JsonNode.Parse(json);
    }

    public static string ToJson<T>(T data, bool indent = IndentByDefault)
    {
      JsonSerializerOptions options = indent ? Json.IndentedJsonOptions : Json.UnindentedJsonOptions;
      return JsonSerializer.Serialize(data, options);
    }

    public const bool IndentByDefault = true;

    public static JsonSerializerOptions ConfigureJson(bool indent) => new JsonSerializerOptions()
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.Never,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
      Converters = { new JsonStringEnumConverter(new UpperSnakeNamingPolicy()) },
      PropertyNameCaseInsensitive = true,
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
      WriteIndented = indent,
    };

    public static readonly JsonSerializerOptions UnindentedJsonOptions = ConfigureJson(indent: false);
    public static readonly JsonSerializerOptions IndentedJsonOptions = ConfigureJson(indent: true);

    public class UpperSnakeNamingPolicy : JsonNamingPolicy
    { public override string ConvertName(string name) => name.ToUpper(); }
  }
}
