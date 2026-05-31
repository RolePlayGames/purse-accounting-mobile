using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DistributionStrategyType
{
    Automatic,
    DoNotNeed,
    UserChoice,
}
