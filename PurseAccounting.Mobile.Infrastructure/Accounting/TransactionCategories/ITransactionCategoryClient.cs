namespace PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;

public interface ITransactionCategoryClient
{
    /// <summary>
    /// Gets user transaction categories
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction categories</returns>
    Task<IReadOnlyCollection<TransactionCategoryDto>> Get(CancellationToken cancellationToken);
}
