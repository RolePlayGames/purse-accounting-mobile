namespace PurseAccounting.Mobile.Infrastructure.Transactions;

public interface ITransactionsClient
{
    /// <summary>
    /// Gets transactions from server
    /// </summary>
    /// <param name="categoryIds">Optional category IDs filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of transactions or empty list on fail</returns>
    Task<IReadOnlyCollection<TransactionInfo>> GetTransactions(IReadOnlyCollection<long>? categoryIds = null, CancellationToken cancellationToken = default);
}
