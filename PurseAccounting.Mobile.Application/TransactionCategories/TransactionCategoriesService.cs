using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;

namespace PurseAccounting.Mobile.Application.TransactionCategories;

internal class TransactionCategoriesService : ITransactionCategoriesService
{
    private readonly ITransactionCategoryClient _transactionCategoryClient;
    private readonly IApplicationContext _applicationContext;

    public TransactionCategoriesService(ITransactionCategoryClient transactionCategoryClient, IApplicationContext applicationContext)
    {
        _transactionCategoryClient = transactionCategoryClient;
        _applicationContext = applicationContext;
    }

    public async Task<IReadOnlyCollection<TransactionCategoryDto>> LoadCategories(CancellationToken cancellationToken)
    {
        return _applicationContext.TransactionCategories = await _transactionCategoryClient.Get(cancellationToken);
    }
}
