namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class AccountWidget : ContentView
{
    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(AccountWidget), default(ImageSource));

    public static readonly BindableProperty MainAmountTextProperty =
        BindableProperty.Create(nameof(MainAmountText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty AvailableAmountTextProperty =
        BindableProperty.Create(nameof(AvailableAmountText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty DayTextProperty =
        BindableProperty.Create(nameof(DayText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty DayNumberProperty = BindableProperty.Create(nameof(DayNumber), typeof(int),
        typeof(AccountWidget), defaultValue: 1, propertyChanged: OnDayNumberChanged);

    public static readonly BindableProperty MainAmountProperty =
        BindableProperty.Create(
            nameof(MainAmount),
            typeof(int),
            typeof(AccountWidget),
            defaultValue: 0,
            propertyChanged: OnAmountChanged);

    public int MainAmount
    {
        get => (int)GetValue(MainAmountProperty);
        set => SetValue(MainAmountProperty, value);
    }

    public static readonly BindableProperty AvailableAmountProperty =
        BindableProperty.Create(
            nameof(AvailableAmount),
            typeof(int),
            typeof(AccountWidget),
            defaultValue: 0,
            propertyChanged: OnAmountChanged);

    public int AvailableAmount
    {
        get => (int)GetValue(AvailableAmountProperty);
        set => SetValue(AvailableAmountProperty, value);
    }

    public static readonly BindableProperty MainAmountFormattedProperty =
        BindableProperty.Create(nameof(MainAmountFormatted), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty AvailableAmountFormattedProperty =
        BindableProperty.Create(nameof(AvailableAmountFormatted), typeof(string), typeof(AccountWidget), string.Empty);

    public string MainAmountFormatted
    {
        get => (string)GetValue(MainAmountFormattedProperty);
        private set => SetValue(MainAmountFormattedProperty, value);
    }

    public string AvailableAmountFormatted
    {
        get => (string)GetValue(AvailableAmountFormattedProperty);
        private set => SetValue(AvailableAmountFormattedProperty, value);
    }

    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public string MainAmountText
    {
        get => (string)GetValue(MainAmountTextProperty);
        set => SetValue(MainAmountTextProperty, value);
    }

    public string AvailableAmountText
    {
        get => (string)GetValue(AvailableAmountTextProperty);
        set => SetValue(AvailableAmountTextProperty, value);
    }

    public string DayText
    {
        get => (string)GetValue(DayTextProperty);
        private set => SetValue(DayTextProperty, value);
    }

    public int DayNumber
    {
        get => (int)GetValue(DayNumberProperty);
        set => SetValue(DayNumberProperty, value);
    }

    public AccountWidget()
    {
        InitializeComponent();
    }

    private static void OnDayNumberChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountWidget card && newValue is int day)
        {
            card.UpdateDayText(day);
        }
    }

    private static string GetDaySuffix(int n)
    {
        var lastDigit = n % 10;
        var lastTwoDigits = n % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            return "дней";

        return lastDigit switch
        {
            1 => "день",
            2 or 3 or 4 => "дн€",
            _ => "дней",
        };
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountWidget card)
        {
            card.UpdateFormattedAmounts();
        }
    }

    private static string FormatAmount(int amount)
    {
        // »спользуем культуру с пробелом как разделителем тыс€ч
        return amount.ToString("#,0", new System.Globalization.CultureInfo("ru-RU"));
    }

    private void UpdateDayText(int day)
    {
        var suffix = GetDaySuffix(day);
        DayText = $"на {day} {suffix}";
    }

    private void UpdateFormattedAmounts()
    {
        MainAmountFormatted = FormatAmount(MainAmount);
        AvailableAmountFormatted = FormatAmount(AvailableAmount);
    }
}
