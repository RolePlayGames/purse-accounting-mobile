using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Daily;

public interface IDailyTransactionClient
{
    /// <summary>
    /// Adds income daily transaction to account
    /// </summary>
    /// <param name="request">Transacction data</param>
    /// <returns>New account amount or exception</returns>
    Task<ApiResult<AddTransactionResponse>> AddIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Adds withdrawal daily transaction to account
    /// </summary>
    /// <param name="request">Transacction data</param>
    /// <returns>New account amount or exception</returns>
    Task<ApiResult<AddTransactionResponse>> AddWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Cancels and reverses transaction from account
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>New account amount</returns>
    Task<ApiResult<CancelTransactionResponse>> CancelTransaction(long transactionId, CancellationToken cancellationToken);
}
