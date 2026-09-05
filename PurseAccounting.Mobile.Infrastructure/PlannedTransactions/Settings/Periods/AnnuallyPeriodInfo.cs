namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

public record AnnuallyPeriodInfo : PeriodInfo
{
    public required int Month { get; init; }

    public required int Day { get; init; }
}
