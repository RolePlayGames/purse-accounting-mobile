using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

/// <summary>
/// Information about distribution strategy
/// </summary>
[JsonConverter(typeof(DistributionStrategyInfoConverter))]
public record DistributionStrategyInfo
{
    public required DistributionStrategyType Type { get; init; }
}
