namespace PurseAccounting.Mobile.Infrastructure.Transactions;

public readonly record struct TransactionInfo
{
    public required long ID { get; init; }

    public required int Amount { get; init; }

    public required DateTime Date { get; init; }

    public required string ChangeAmountType { get; init; }

    public required long TransactionCategoryID { get; init; }
}
