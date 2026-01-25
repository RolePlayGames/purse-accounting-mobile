using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.TomorrowAmountCalculators;

public interface ITomorrowAmountCalculator
{
    /// <summary>
    /// Calculates tomorrow amount
    /// </summary>
    /// <param name="account">Account</param>
    /// <param name="calculationStrategy">Strategy to calculate</param>
    /// <returns>Tomorrow amount</returns>
    long Calculate(Account account, TomorrowAmountCalculationStrategy calculationStrategy);
}
