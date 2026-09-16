using System.Globalization;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Extensions;

namespace PurseAccountinng.Mobile.Presentation.Services.Utils;

internal static class AmountFormatter
{
    private static readonly CultureInfo _culture = new("ru-RU");

    /// <summary>
    /// Formats amount to separate thousands and fractional part
    /// </summary>
    /// <param name="amount">Not formatted amount</param>
    /// <returns>Formatted amount string</returns>
    public static string FormatAmount(long amount)
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

    /// <summary>
    /// Formats transaction amount with sign and color based on transaction type
    /// </summary>
    /// <param name="amount">Transaction amount</param>
    /// <param name="changeType">Optional change type, if null determined by amount sign</param>
    /// <returns>Tuple of formatted text and text color</returns>
    public static (string Text, Color TextColor) FormatTransactionAmount(int amount, TransactionChangeType? changeType = null)
    {
        var formattedAmount = FormatAmount(Math.Abs(amount));
        var actualChangeType = changeType ?? (amount >= 0 ? TransactionChangeType.Income : TransactionChangeType.Withdrawal);
        var amountSign = actualChangeType == TransactionChangeType.Income ? '+' : '-';

        var text = $"{amountSign} {formattedAmount} ₽";
        var textColor = (actualChangeType == TransactionChangeType.Income
                ? App.Current?.Resources.GetColor("TransactionPositive")
                : App.Current?.Resources.GetColor("Gray1"))
            ?? Microsoft.Maui.Graphics.Colors.Black;

        return (text, textColor);
    }
}
