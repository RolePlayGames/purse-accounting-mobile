using PurseAccounting.Mobile.Infrastructure.Distribution;

namespace PurseAccounting.Mobile.Application.Distribution;

public interface IDistributionService
{
    /// <summary>
    /// Gets current distribution strategy
    /// </summary>
    /// <returns>Distribution strategy info</returns>
    Task<DistributionStrategyInfo> GetDistributionStrategy(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes all amount to today
    /// </summary>
    /// <returns>Is operation succeeded</returns>
    Task<bool> DistributeAllToToday(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes amount between days
    /// </summary>
    /// <returns>Is operation succeeded</returns>
    Task<bool> DistributeBetweenDays(CancellationToken cancellationToken);
}
