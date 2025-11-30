using PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;
using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.AmountsDistributionCalculators;

internal class AmountsDistributionCalculator : IAmountsDistributionCalculator
{
    private readonly IAmountsCalculator _amountsCalculator;

    public AmountsDistributionCalculator(IAmountsCalculator amountsCalculator)
    {
        _amountsCalculator = amountsCalculator;
    }

    public DailyDistributedAmount CalculateDistributionToToday(Account account)
    {
        var dailyDistributedAmount = _amountsCalculator.CalculateAmounts(account.AvaliableAmount, account.DaysCount - 1);

        return new()
        {
            DayAmount = account.DayAmount,
            RestAmount = dailyDistributedAmount.RestAmount + dailyDistributedAmount.DayAmount,
        };
    }

    public DailyDistributedAmount CalculateDistributionBetweenDays(Account account)
    {
        return _amountsCalculator.CalculateAmounts(account.AvaliableAmount, account.DaysCount - 1);
    }
}
