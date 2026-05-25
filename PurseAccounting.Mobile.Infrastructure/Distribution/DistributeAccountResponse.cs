namespace PurseAccounting.Mobile.Infrastructure.Distribution;

/// <summary>
/// Account state after distribution was made
/// </summary>
public record DistributeAccountResponse
{
    public required long RestAmount { get; init; }

    public required long DayAmount { get; init; }

    public required DateTime PlannedDate { get; init; }

    public required short TimeZone { get; init; }
}
