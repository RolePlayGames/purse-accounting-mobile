namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Account state after transaction was made
/// </summary>
public record AddTransactionResponse
{
    public required long RestAmount { get; init; }

    public required long DayAmount { get; init; }

    public required long ReservedAmount { get; init; }
}
