using PurseAccounting.Mobile.Infrastructure.ApiResults;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Daily;

public interface IDailyTransactionClient
{
    Task<ApiResult<AddTransactionResponse>> AddDailyIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken);

    Task<ApiResult<AddTransactionResponse>> AddDailyWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken);
}
