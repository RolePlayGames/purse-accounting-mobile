namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Awaiting planned transaction information
/// </summary>
public record AwaitingPlannedTransactionInfo
{
    public required long ID { get; init; }

    public required int Amount { get; init; }

    public required DateTime AwaitingDate { get; init; }

    public required long TransactionCategoryID { get; init; }

    public required TransactionChangeType ChangeType { get; init; }
}
