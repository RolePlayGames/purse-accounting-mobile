namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

public record WeeklyPeriodInfo : PeriodInfo
{
    public required DayOfWeek[] DaysOfWeek { get; init; }
}
