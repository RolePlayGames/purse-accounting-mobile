namespace PurseAccounting.Mobile.Application.Calculators.DaysCountCalculators;

internal class DaysCountCalculators : IDaysCountCalculators
{
    public int Calculate(DateTime from, DateTime to)
    {
        return Math.Max((to - from).Days + 1, 0);
    }
}
