namespace PurseAccounting.Mobile.Application.Models;

public record DailyDistributedAmount
{
    public required long DayAmount { get; init; }

    public required long RestAmount { get; init; }

    public required long ReservedAmount { get; init; }
}
