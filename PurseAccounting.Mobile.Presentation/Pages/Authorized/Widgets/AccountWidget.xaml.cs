using PurseAccountinng.Mobile.Presentation.Services.Utils;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Widgets;

public partial class AccountWidget : ContentView
{
    public static readonly BindableProperty DayAmountTextProperty =
        BindableProperty.Create(nameof(DayAmountText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty AvailableAmountTextProperty =
        BindableProperty.Create(nameof(AvailableAmountText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty DaysCountTextProperty =
        BindableProperty.Create(nameof(DaysCountText), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty DaysCountProperty = BindableProperty.Create(nameof(DaysCount), typeof(int),
        typeof(AccountWidget), defaultValue: 1, propertyChanged: OnDayNumberChanged);

    public static readonly BindableProperty DayAmountProperty =
        BindableProperty.Create(
            nameof(DayAmount),
            typeof(int),
            typeof(AccountWidget),
            defaultValue: 0,
            propertyChanged: OnAmountChanged);

    public static readonly BindableProperty AvailableAmountProperty =
        BindableProperty.Create(
            nameof(AvailableAmount),
            typeof(int),
            typeof(AccountWidget),
            defaultValue: 0,
            propertyChanged: OnAmountChanged);

    public static readonly BindableProperty DayAmountFormattedProperty =
        BindableProperty.Create(nameof(DayAmountFormatted), typeof(string), typeof(AccountWidget), string.Empty);

    public static readonly BindableProperty AvailableAmountFormattedProperty =
        BindableProperty.Create(nameof(AvailableAmountFormatted), typeof(string), typeof(AccountWidget), string.Empty);

    public int DayAmount
    {
        get => (int)GetValue(DayAmountProperty);
        set => SetValue(DayAmountProperty, value);
    }

    public int AvailableAmount
    {
        get => (int)GetValue(AvailableAmountProperty);
        set => SetValue(AvailableAmountProperty, value);
    }

    public string DayAmountFormatted
    {
        get => (string)GetValue(DayAmountFormattedProperty);
        private set => SetValue(DayAmountFormattedProperty, value);
    }

    public string AvailableAmountFormatted
    {
        get => (string)GetValue(AvailableAmountFormattedProperty);
        private set => SetValue(AvailableAmountFormattedProperty, value);
    }

    public string DayAmountText
    {
        get => (string)GetValue(DayAmountTextProperty);
        set => SetValue(DayAmountTextProperty, value);
    }

    public string AvailableAmountText
    {
        get => (string)GetValue(AvailableAmountTextProperty);
        set => SetValue(AvailableAmountTextProperty, value);
    }

    public string DaysCountText
    {
        get => (string)GetValue(DaysCountTextProperty);
        private set => SetValue(DaysCountTextProperty, value);
    }

    public int DaysCount
    {
        get => (int)GetValue(DaysCountProperty);
        set => SetValue(DaysCountProperty, value);
    }

    public AccountWidget()
    {
        InitializeComponent();
    }

    private static void OnDayNumberChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountWidget card && newValue is int day)
        {
            card.UpdateDaysCountText(day);
        }
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountWidget card)
        {
            card.UpdateFormattedAmounts();
        }
    }

    private void UpdateDaysCountText(int daysCount)
    {
        DaysCountText = DaysCountFormatter.FormatDaysCount(daysCount);
    }

    private void UpdateFormattedAmounts()
    {
        DayAmountFormatted = AmountFormatter.FormatAmount(DayAmount);
        AvailableAmountFormatted = AmountFormatter.FormatAmount(AvailableAmount);
    }
}
