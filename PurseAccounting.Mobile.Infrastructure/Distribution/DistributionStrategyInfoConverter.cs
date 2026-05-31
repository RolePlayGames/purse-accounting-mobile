using System.Text.Json;
using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

public class DistributionStrategyInfoConverter : JsonConverter<DistributionStrategyInfo>
{
    private static readonly JsonSerializerOptions _readOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public override DistributionStrategyInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string? typeValue = null;

        foreach (var prop in root.EnumerateObject())
        {
            if (string.Equals(prop.Name, "type", StringComparison.OrdinalIgnoreCase))
            {
                typeValue = prop.Value.GetString();
                break;
            }
        }

        if (typeValue is null)
            throw new JsonException("Missing required 'type' property.");

        if (!Enum.TryParse<DistributionStrategyType>(typeValue, ignoreCase: true, out var strategyType))
            throw new JsonException($"Unknown strategy type: '{typeValue}'");

        return strategyType switch
        {
            DistributionStrategyType.Automatic => root.Deserialize<DistributionStrategyInfo>(_readOptions),
            DistributionStrategyType.DoNotNeed => root.Deserialize<DistributionStrategyInfo>(_readOptions),
            DistributionStrategyType.UserChoice => root.Deserialize<UserChoiceDistributionStrategyInfo>(_readOptions),
            _ => throw new JsonException($"Unsupported strategy type: {strategyType}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, DistributionStrategyInfo value, JsonSerializerOptions options)
    {
        // При сериализации можно делегировать стандартному механизму или атрибутам
        JsonSerializer.Serialize(writer, value, options);
    }
}
