namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Request to add transaction
/// </summary>
public record AddTransactionRequest
{
    public required int Amount { get; init; }

    public required long TransactionCategoryID { get; init; }
}
