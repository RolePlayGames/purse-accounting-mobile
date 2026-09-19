namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

public record OncePeriodInfo : PeriodInfo
{
    public required DateTime Date { get; init; }
}
