using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;

/// <summary>
/// Manages amounts calculation logic
/// </summary>
internal interface IAmountsCalculator
{
    /// <summary>
    /// Calculates day amount and rest amount for date interval
    /// </summary>
    /// <param name="totalAmount">Total amount</param>
    /// <param name="daysCount">Days count</param>
    /// <returns>Amounts for days</returns>
    DailyDistributedAmount CalculateAmounts(long totalAmount, int daysCount);
}
