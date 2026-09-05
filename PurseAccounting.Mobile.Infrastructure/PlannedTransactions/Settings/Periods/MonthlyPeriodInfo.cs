namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

public record MonthlyPeriodInfo : PeriodInfo
{
    public required int Day { get; init; }
}
