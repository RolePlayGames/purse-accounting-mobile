using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;

namespace PurseAccounting.Mobile.Application.TransactionCategories;

public interface ITransactionCategoriesService
{
    /// <summary>
    /// Loads users transaction categories
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction categories</returns>
    Task<IReadOnlyCollection<TransactionCategoryDto>> LoadCategories(CancellationToken cancellationToken);
}
