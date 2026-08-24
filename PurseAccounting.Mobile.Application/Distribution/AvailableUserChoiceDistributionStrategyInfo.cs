namespace PurseAccounting.Mobile.Application.Distribution;

/// <summary>
/// Information about available user choice distribution strategy
/// </summary>
public record AvailableUserChoiceDistributionStrategyInfo
{
    public required long AllToTodayDistributedDayAmount { get; init; }

    public required long BetweenDaysDistributedDayAmount { get; init; }
}
