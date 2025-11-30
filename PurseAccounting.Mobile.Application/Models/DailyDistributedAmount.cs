namespace PurseAccounting.Mobile.Application.Models;

public record DailyDistributedAmount
{
    public long DayAmount { get; set; }

    public long RestAmount { get; set; }
}
