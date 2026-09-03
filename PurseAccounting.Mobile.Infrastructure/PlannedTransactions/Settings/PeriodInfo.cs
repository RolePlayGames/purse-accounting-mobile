namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

/// <summary>
/// Period information for planned transaction settings
/// </summary>
public record PeriodInfo
{
    public required int Day { get; init; }

    public required int Month { get; init; }

    public required int Year { get; init; }
}
