namespace PurseAccounting.Mobile.Application.Models;

public record Account
{
    private readonly IDateTimeService _dateTimeService;

    public required DailyDistributedAmount DailyDistributedAmount { get; init; }

    public required Date PlannedDate { get; init; }

    public required short TimeZone { get; init; }

    public long DayAmount => DailyDistributedAmount.DayAmount;

    public long AvaliableAmount => DailyDistributedAmount.RestAmount + DailyDistributedAmount.DayAmount;

    public int DaysCount
    {
        get
        {
            var t = _dateTimeService.UtcNow.AddHours(TimeZone).Date;
            var t2 = (PlannedDate - _dateTimeService.UtcNow.AddHours(TimeZone).Date).Days;
            var t3 = Math.Max(t2 + 1, 0);
            return t3;
        }
    }

    public Account(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }
}
