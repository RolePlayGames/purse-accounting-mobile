using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services.Utils;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class AutoPlannedTransactionRow : ContentView
{
    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(AutoPlannedTransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public static readonly BindableProperty AmountProperty =
        BindableProperty.Create(nameof(Amount), typeof(decimal), typeof(AutoPlannedTransactionRow), default(decimal), propertyChanged: OnAmountChanged);

    public static readonly BindableProperty AmountTextProperty =
        BindableProperty.Create(nameof(AmountText), typeof(string), typeof(AutoPlannedTransactionRow), string.Empty);

    public static readonly BindableProperty AmountTextColorProperty =
        BindableProperty.Create(nameof(AmountTextColor), typeof(Color), typeof(AutoPlannedTransactionRow), Microsoft.Maui.Graphics.Colors.Black);

    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(AutoPlannedTransactionRow), string.Empty);

    public static readonly BindableProperty IsIconVisibleProperty =
        BindableProperty.Create(nameof(IsIconVisible), typeof(bool), typeof(AutoPlannedTransactionRow), true);

    public static readonly BindableProperty DescriptionTextProperty =
        BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(AutoPlannedTransactionRow), string.Empty);

    public Brush CircleColor
    {
        get => (Brush)GetValue(CircleColorProperty);
        set => SetValue(CircleColorProperty, value);
    }

    public decimal Amount
    {
        get => (decimal)GetValue(AmountProperty);
        set => SetValue(AmountProperty, value);
    }

    public string AmountText
    {
        get => (string)GetValue(AmountTextProperty);
        set => SetValue(AmountTextProperty, value);
    }

    public Color AmountTextColor
    {
        get => (Color)GetValue(AmountTextColorProperty);
        set => SetValue(AmountTextColorProperty, value);
    }

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public bool IsIconVisible
    {
        get => (bool)GetValue(IsIconVisibleProperty);
        set => SetValue(IsIconVisibleProperty, value);
    }

    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateAmountProperties();
        }
    }

    public AutoPlannedTransactionRow()
    {
        InitializeComponent();
        UpdateAmountProperties();
    }

    private void UpdateAmountProperties()
    {
        var amount = Amount;
        var formattedAmount = AmountFormatter.FormatAmount(Math.Abs(amount));
        var amountSign = amount >= 0 ? '+' : '-';

        AmountText = $"{amountSign} {formattedAmount} ₽";
        AmountTextColor = (amount >= 0 ? App.Current?.Resources.GetColor("TransactionPositive") : App.Current?.Resources.GetColor("Gray1")) ?? AmountTextColor;
    }
}
