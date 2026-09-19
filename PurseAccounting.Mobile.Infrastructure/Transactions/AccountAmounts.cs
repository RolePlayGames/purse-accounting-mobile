namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Account amounts state
/// </summary>
public record AccountAmounts
{
    public required long RestAmount { get; init; }

    public required long DayAmount { get; init; }

    public required long ReservedAmount { get; init; }
}
