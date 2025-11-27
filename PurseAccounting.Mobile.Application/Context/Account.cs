namespace PurseAccounting.Mobile.Application.Context;

public record Account
{
    public required long DayAmount { get; init; }

    public required long AvaliableAmount { get; init; }

    public required int DaysCount { get; init; }
}
