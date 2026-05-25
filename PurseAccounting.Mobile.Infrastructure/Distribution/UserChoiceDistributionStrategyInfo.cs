using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

/// <summary>
/// Information about user choice distribution strategy
/// </summary>
public record UserChoiceDistributionStrategyInfo : DistributionStrategyInfo
{
    public required long AllToTodayDistributedDayAmount { get; init; }

    public required long BetweenDaysDistributedDayAmount { get; init; }
}
