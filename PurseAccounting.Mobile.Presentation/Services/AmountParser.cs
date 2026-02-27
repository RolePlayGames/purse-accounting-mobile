using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Services;

internal class AmountParser
{
    public static string FilterInput(string input)
    {
        input = input.Replace('.', ',');
        var cleaned = new string([.. input.Where(c => char.IsDigit(c) || c == ',')]);

        var parts = cleaned.Split(',');

        if (parts.Length > 2)
            cleaned = parts[0] + "," + string.Concat(parts.Skip(1));

        if (cleaned.Contains(','))
        {
            var idx = cleaned.IndexOf(',');
            var intPart = cleaned[..idx];
            var fracPart = cleaned[(idx + 1)..];

            if (fracPart.Length > 2)
                fracPart = fracPart[..2];

            cleaned = intPart + "," + fracPart;
        }

        return cleaned;
    }

    public static bool TryParseToCents(string input, out int cents)
    {
        cents = 0;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (input.EndsWith(','))
            return false;

        var normalized = input.Replace(',', '.');

        if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            return false;

        if (value <= 0)
            return false;

        value = Math.Round(value, 2);
        cents = (int)(value * 100m);

        return cents >= 0;
    }
}
