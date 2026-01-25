using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;

internal class AmountsCalculator : IAmountsCalculator
{
    public DailyDistributedAmount CalculateAmounts(long totalAmount, int daysCount)
    {
        if (totalAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount should be greater than or equals to 0");

        if (daysCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(daysCount), "Days count should be greater than 0");

        var dayAmount = totalAmount / daysCount;
        var restAmount = totalAmount - dayAmount;

        return new() { DayAmount = dayAmount, RestAmount = restAmount };
    }
}
