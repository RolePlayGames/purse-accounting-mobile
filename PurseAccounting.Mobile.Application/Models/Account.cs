namespace PurseAccounting.Mobile.Application.Models;

public record Account
{
    private readonly IDateTimeService _dateTimeService;

    public required DailyDistributedAmount DailyDistributedAmount { get; init; }

    public required Date PlannedDate { get; init; }

    public required short TimeZone { get; init; }

    public long DayAmount => DailyDistributedAmount.DayAmount;

    public long AvaliableAmount => DailyDistributedAmount.RestAmount + DailyDistributedAmount.DayAmount;

    public int DaysCount => Math.Max((PlannedDate - _dateTimeService.UtcNow.AddHours(TimeZone).Date).Days + 1, 0);

    public Account(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }
}
