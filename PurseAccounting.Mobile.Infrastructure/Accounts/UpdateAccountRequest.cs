namespace PurseAccounting.Mobile.Infrastructure.Accounts;

public record UpdateAccountRequest
{
    public required long FullAmount { get; init; }

    public required DateTime PlannedDate { get; init; }

    public required short TimeZone { get; init; }
}
