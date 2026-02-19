namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Transaction's type, indicating how the amount is changed
/// </summary>
public enum TransactionChangeType
{
    /// <summary>
    /// Income transaction
    /// </summary>
    Income,

    /// <summary>
    /// Withdrawal transaction
    /// </summary>
    Withdrawal,
}
