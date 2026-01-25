using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.AmountsDistributionCalculators;

/// <summary>
/// Calculates amounts after distribution rest daily amount
/// </summary>
public interface IAmountsDistributionCalculator
{
    /// <summary>
    /// Calculates amounts distributuion to current day
    /// </summary>
    /// <param name="account">Account to distribute</param>
    /// <returns>Account amounts</returns>
    public DailyDistributedAmount CalculateDistributionToToday(Account account);

    /// <summary>
    /// Calculates amounts distributuion between all days
    /// </summary>
    /// <param name="account">Account to distribute</param>
    /// <returns>Account amounts</returns>
    public DailyDistributedAmount CalculateDistributionBetweenDays(Account account);
}
