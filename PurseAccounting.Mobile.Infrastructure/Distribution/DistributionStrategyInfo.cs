namespace PurseAccounting.Mobile.Infrastructure.Distribution;

/// <summary>
/// Information about distribution strategy
/// </summary>
public record DistributionStrategyInfo
{
    public required string Type { get; init; }
}

/// <summary>
/// Information about user choice distribution strategy
/// </summary>
public record UserChoiceDistributionStrategyInfo : DistributionStrategyInfo
{
    public required long AllToTodayDistributedDayAmount { get; init; }

    public required long BetweenDaysDistributedDayAmount { get; init; }
}
