namespace PurseAccounting.Mobile.Application.Distribution;

public interface IDistributionService
{
    /// <summary>
    /// Gets available user choice distribution strategy info, or null if automatic/do not need
    /// </summary>
    /// <returns>Available user choice distribution strategy info or null</returns>
    Task<AvailableUserChoiceDistributionStrategyInfo?> GetAvailableUserChoiceDistributionStrategy(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes all amount to today
    /// </summary>
    /// <returns>Is operation succeeded</returns>
    Task<DistributionResult> DistributeAllToToday(CancellationToken cancellationToken);

    /// <summary>
    /// Distributes amount between days
    /// </summary>
    /// <returns>Is operation succeeded</returns>
    Task<DistributionResult> DistributeBetweenDays(CancellationToken cancellationToken);
}
