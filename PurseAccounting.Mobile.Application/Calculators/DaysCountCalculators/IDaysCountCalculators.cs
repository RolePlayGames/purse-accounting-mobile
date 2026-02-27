namespace PurseAccounting.Mobile.Application.Calculators.DaysCountCalculators;

public interface IDaysCountCalculators
{
    /// <summary>
    /// Calculates budget days count between two dates
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="to">Date to</param>
    /// <returns>Days count</returns>
    int Calculate(DateTime from, DateTime to);
}
