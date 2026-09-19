namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Request to change awaiting planned transaction amount
/// </summary>
public record ChangeAwaitingPlannedTransactionAmountRequest
{
    public required long AwaitingPlannedTransactionID { get; init; }

    public required int Amount { get; init; }
}
