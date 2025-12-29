using PurseAccounting.Mobile.Infrastructure.ApiResults;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Total;

public interface ITotalTransactionClient
{
    Task<ApiResult<AddTransactionResponse>> AddTotalIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken);

    Task<ApiResult<AddTransactionResponse>> AddTotalWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken);
}
