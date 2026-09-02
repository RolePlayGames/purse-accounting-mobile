using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Awaiting;

public interface IAwaitingPlannedTransactionClient
{
    /// <summary>
    /// Gets all awaiting planned transactions
    /// </summary>
    /// <returns>List of awaiting planned transactions</returns>
    Task<ApiResult<IReadOnlyCollection<AwaitingPlannedTransactionInfo>>> GetAwaitingPlannedTransactions(CancellationToken cancellationToken);

    /// <summary>
    /// Changes amount of an awaiting planned transaction
    /// </summary>
    /// <param name="request">Request with transaction ID and new amount</param>
    /// <returns>New account amounts</returns>
    Task<ApiResult<AccountAmounts>> ChangeAmount(ChangeAwaitingPlannedTransactionAmountRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Applies an awaiting planned transaction
    /// </summary>
    /// <param name="awaitingPlannedTransactionId">Transaction ID to apply</param>
    /// <returns>New account amounts</returns>
    Task<ApiResult<AccountAmounts>> ApplyTransaction(long awaitingPlannedTransactionId, CancellationToken cancellationToken);

    /// <summary>
    /// Declines an awaiting planned transaction
    /// </summary>
    /// <param name="awaitingPlannedTransactionId">Transaction ID to decline</param>
    /// <returns>New account amounts</returns>
    Task<ApiResult<AccountAmounts>> DeclineTransaction(long awaitingPlannedTransactionId, CancellationToken cancellationToken);
}
