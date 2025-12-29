using System.Globalization;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class TransactionAmountInput : ContentView
{
    public static readonly BindableProperty SubmitCommandProperty
        = BindableProperty.Create(nameof(SubmitCommand), typeof(ICommand), typeof(TransactionAmountInput), null);

    public static readonly BindableProperty AmountProperty
        = BindableProperty.Create(propertyName: nameof(Amount), returnType: typeof(int?), declaringType: typeof(TransactionAmountInput), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnAmountChanged);

    public ICommand? SubmitCommand
    {
        get => (ICommand?)GetValue(SubmitCommandProperty);
        set => SetValue(SubmitCommandProperty, value);
    }

    public int? Amount
    {
        get => (int?)GetValue(AmountProperty);
        set => SetValue(AmountProperty, value);
    }

    public TransactionAmountInput()
    {
        InitializeComponent();
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TransactionAmountInput control)
            return;

        var amountInCents = (int?)newValue;

        var text = amountInCents.HasValue switch
        {
            true => (amountInCents.Value / 100m).ToString("0.##", CultureInfo.CreateSpecificCulture("ru-RU")),
            false => string.Empty,
        };

        if (control.AmountEntry.Text != text)
            control.AmountEntry.Text = text;
    }

    private static string FilterInput(string input)
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

    private static bool TryParseToCents(string input, out int cents)
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

    private void OnAmountTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        var input = e.NewTextValue ?? string.Empty;
        var sanitized = FilterInput(input);

        if (sanitized != input)
        {
            entry.TextChanged -= OnAmountTextChanged;
            entry.Text = sanitized;
            entry.TextChanged += OnAmountTextChanged;
            entry.CursorPosition = entry.Text.Length;
        }

        if (TryParseToCents(sanitized, out var cents) && Amount != cents)
            Amount = cents;
    }
}
