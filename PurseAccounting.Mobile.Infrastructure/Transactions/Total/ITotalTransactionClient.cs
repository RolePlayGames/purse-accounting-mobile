using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Total;

public interface ITotalTransactionClient
{
    /// <summary>
    /// Adds income total transaction to account
    /// </summary>
    /// <param name="request">Transacction data</param>
    /// <returns>New account amount or exception</returns>
    Task<ApiResult<AddTransactionResponse>> AddTotalIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Adds income total transaction to account
    /// </summary>
    /// <param name="request">Transacction data</param>
    /// <returns>New account amount or exception</returns>
    Task<ApiResult<AddTransactionResponse>> AddTotalWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken);
}
