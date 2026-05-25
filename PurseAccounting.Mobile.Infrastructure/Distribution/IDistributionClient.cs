using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

public interface IDistributionClient
{
    /// <summary>
    /// Gets current distribution strategy
    /// </summary>
    /// <returns>Distribution strategy info</returns>
    Task<ApiResult<DistributionStrategyInfo>> GetDistributionStrategy(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes all amount to today
    /// </summary>
    /// <returns>Account state after distribution</returns>
    Task<ApiResult<DistributeAccountResponse>> DistributeAllToToday(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes amount automatically based on strategy
    /// </summary>
    /// <returns>Account state after distribution</returns>
    Task<ApiResult<DistributeAccountResponse>> DistributeAutomatically(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes amount between days
    /// </summary>
    /// <returns>Account state after distribution</returns>
    Task<ApiResult<DistributeAccountResponse>> DistributeBetweenDays(CancellationToken cancellationToken);
}
