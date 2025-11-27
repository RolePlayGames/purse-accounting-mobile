namespace PurseAccountinng.Mobile.Presentation.Services.Utils;

internal static class DaysCountFormatter
{
    /// <summary>
    /// Formats days count with number and suffix
    /// </summary>
    /// <param name="daysCount">Count of days</param>
    /// <returns>Formatted days cunt string</returns>
    public static string FormatDaysCount(int daysCount)
    {
        var suffix = GetDaysCountSuffix(daysCount);
        return $"на {daysCount} {suffix}";
    }

    private static string GetDaysCountSuffix(int daysCount)
    {
        var lastDigit = daysCount % 10;
        var lastTwoDigits = daysCount % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            return "дней";

        return lastDigit switch
        {
            1 => "день",
            2 or 3 or 4 => "дня",
            _ => "дней",
        };
    }
}
