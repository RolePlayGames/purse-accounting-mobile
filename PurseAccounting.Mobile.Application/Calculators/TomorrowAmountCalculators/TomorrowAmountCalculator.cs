using PurseAccounting.Mobile.Application.Calculators.AmountsDistributionCalculators;
using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Calculators.TomorrowAmountCalculators;

internal class TomorrowAmountCalculator : ITomorrowAmountCalculator
{
    private readonly IAmountsDistributionCalculator _amountsDistributionCalculator;

    public TomorrowAmountCalculator(IAmountsDistributionCalculator amountsDistributionCalculator)
    {
        _amountsDistributionCalculator = amountsDistributionCalculator;
    }

    public long Calculate(Account account, TomorrowAmountCalculationStrategy calculationStrategy)
    {
        if (account.AvaliableAmount <= 0)
            return 0;

        var distributedAmounts = calculationStrategy == TomorrowAmountCalculationStrategy.AllToToday
            ? _amountsDistributionCalculator.CalculateDistributionToToday(account)
            : _amountsDistributionCalculator.CalculateDistributionBetweenDays(account);

        return distributedAmounts.DayAmount;
    }
}
