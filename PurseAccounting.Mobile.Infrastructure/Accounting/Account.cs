namespace PurseAccounting.Mobile.Infrastructure.Accounting;

public record Account
{
    public required long RestAmount { get; init; }

    public required long DayAmount { get; init; }

    public required DateTime PlannedDate { get; init; }

    public required short TimeZone { get; init; }
}
