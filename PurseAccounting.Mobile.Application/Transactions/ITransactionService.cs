using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.Transactions;

public interface ITransactionService
{
    /// <summary>
    /// Makes transaction
    /// </summary>
    /// <param name="transaction">Transactio data</param>
    /// <returns>Is operation succeeded</returns>
    Task<MakeTransactionResult> MakeTransaction(Transaction transaction, CancellationToken cancellationToken);

    /// <summary>
    /// Gets filtered by categories transactions
    /// </summary>
    /// <param name="categoryIds">Categories filter (can be empty or null to get without filter)</param>
    /// <param name="timeZone">Account timezone</param>
    /// <returns>Grouped by date transactions</returns>
    Task<IReadOnlyCollection<TransactionGroup>> GetTransactionsByDate(IReadOnlyCollection<long> categoryIds, short timeZone, CancellationToken cancellationToken);

    /// <summary>
    /// Cancels and reverses transaction
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <param name="changeAmountType">Transaction amount type</param>
    /// <returns>Is operation succeeded</returns>
    Task<bool> CancelTransaction(long transactionId, TransactionChangeAmountType changeAmountType, CancellationToken cancellationToken);
}
