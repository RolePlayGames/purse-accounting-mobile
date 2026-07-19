namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Account state after transaction canceled
/// </summary>
public record CancelTransactionResponse
{
    public required long RestAmount { get; init; }

    public required long DayAmount { get; init; }
}
