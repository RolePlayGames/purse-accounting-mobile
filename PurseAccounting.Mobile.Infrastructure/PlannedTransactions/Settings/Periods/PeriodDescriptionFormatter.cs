namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

/// <summary>
/// Helper class for formatting period descriptions
/// </summary>
public static class PeriodDescriptionFormatter
{
    private static readonly Dictionary<DayOfWeek, string> WEEKDAY_SHORT_NAMES = new()
    {
        { DayOfWeek.Sunday, "Вс" },
        { DayOfWeek.Monday, "Пн" },
        { DayOfWeek.Tuesday, "Вт" },
        { DayOfWeek.Wednesday, "Ср" },
        { DayOfWeek.Thursday, "Чт" },
        { DayOfWeek.Friday, "Пт" },
        { DayOfWeek.Saturday, "Сб" },
    };

    private static readonly string[] MONTH_NAMES_GENITIVE =
    [
        "января",
        "февраля",
        "марта",
        "апреля",
        "мая",
        "июня",
        "июля",
        "августа",
        "сентября",
        "октября",
        "ноября",
        "декабря",
    ];

    /// <summary>
    /// Normalizes day of week number to start from Monday
    /// </summary>
    /// <param name="day">Week day</param>
    /// <returns>0 for Monday etc.</returns>
    private static int NormalizeDayOfWeek(DayOfWeek day) => day == DayOfWeek.Sunday ? 6 : (int)day - 1;

    /// <summary>
    /// Formats week day list, uniting week day sequences
    /// </summary>
    /// <param name="days">Week days</param>
    /// <returns>Formatted string</returns>
    private static string FormatDaysOfWeek(DayOfWeek[] days)
    {
        if (days.Length == 0)
            return string.Empty;

        var sortedDays = days.OrderBy(d => NormalizeDayOfWeek(d)).ToArray();

        var ranges = new List<(DayOfWeek Start, DayOfWeek End)>();
        var currentRange = (Start: sortedDays[0], End: sortedDays[0]);

        for (int i = 1; i < sortedDays.Length; i++)
        {
            var currentDay = sortedDays[i];
            var previousDay = sortedDays[i - 1];

            if (NormalizeDayOfWeek(currentDay) == NormalizeDayOfWeek(previousDay) + 1)
                currentRange.End = currentDay;
            else
            {
                ranges.Add(currentRange);
                currentRange = (Start: currentDay, End: currentDay);
            }
        }

        ranges.Add(currentRange);

        return string.Join(", ", ranges.Select(range =>
        {
            var length = NormalizeDayOfWeek(range.End) - NormalizeDayOfWeek(range.Start) + 1;

            if (length == 1)
                return WEEKDAY_SHORT_NAMES[range.Start];
            else if (length == 2)
                return $"{WEEKDAY_SHORT_NAMES[range.Start]}, {WEEKDAY_SHORT_NAMES[range.End]}";
            else
                return $"{WEEKDAY_SHORT_NAMES[range.Start]}-{WEEKDAY_SHORT_NAMES[range.End]}";
        }));
    }

    /// <summary>
    /// Gets period description based on the specific period type
    /// </summary>
    /// <param name="period">Period to get description</param>
    /// <returns>Period description</returns>
    public static string GetDescription(PeriodInfo period)
    {
        return period switch
        {
            DailyPeriodInfo => "Ежедневно",
            WeeklyPeriodInfo weekly => $"Еженедельно - {FormatDaysOfWeek(weekly.DaysOfWeek)}",
            OncePeriodInfo once => once.Date.ToString("dd.MM.yyyy"),
            MonthlyPeriodInfo monthly => $"Ежемесячно - {monthly.Day} число",
            AnnuallyPeriodInfo annually => $"Ежегодно - {annually.Day} {MONTH_NAMES_GENITIVE[annually.Month - 1]}",
            _ => string.Empty
        };
    }
}
