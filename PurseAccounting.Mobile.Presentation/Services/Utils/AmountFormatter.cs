using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Services.Utils;

internal static class AmountFormatter
{
    private static readonly CultureInfo _culture = new("ru-RU");

    /// <summary>
    /// Formats amount to separate thousands and fractional part
    /// </summary>
    /// <param name="amount">Not formatted amount</param>
    /// <returns>Formatted amount string</returns>
    public static string FormatAmount(int amount)
    {
        if (amount % 100 == 0)
        {
            var rubles = amount / 100;
            return rubles.ToString("#,0", _culture);
        }
        else
        {
            var rubles = amount / 100m;
            return rubles.ToString("#,0.00", _culture);
        }
    }
}
