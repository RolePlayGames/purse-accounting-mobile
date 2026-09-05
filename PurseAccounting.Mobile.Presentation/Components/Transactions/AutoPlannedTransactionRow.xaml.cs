using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;
using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services.Utils;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class AutoPlannedTransactionRow : ContentView
{
    public static readonly BindableProperty PlannedTransactionSettingInfoProperty =
        BindableProperty.Create(nameof(PlannedTransactionSettingInfo), typeof(PlannedTransactionSettingInfo), typeof(AutoPlannedTransactionRow), default(PlannedTransactionSettingInfo), propertyChanged: OnPlannedTransactionSettingInfoChanged);

    public static readonly BindableProperty TransactionCategoryIDProperty =
        BindableProperty.Create(nameof(TransactionCategoryID), typeof(long?), typeof(AutoPlannedTransactionRow), default(long?), propertyChanged: OnTransactionCategoryIDOrCategoriesChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(AutoPlannedTransactionRow), default(IReadOnlyDictionary<long, TransactionCategoryDto>), propertyChanged: OnTransactionCategoryIDOrCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(AutoPlannedTransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public static readonly BindableProperty AmountProperty =
        BindableProperty.Create(nameof(Amount), typeof(int), typeof(AutoPlannedTransactionRow), default(int), propertyChanged: OnAmountChanged);

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

    public PlannedTransactionSettingInfo? PlannedTransactionSettingInfo
    {
        get => (PlannedTransactionSettingInfo?)GetValue(PlannedTransactionSettingInfoProperty);
        set => SetValue(PlannedTransactionSettingInfoProperty, value);
    }

    public long? TransactionCategoryID
    {
        get => (long?)GetValue(TransactionCategoryIDProperty);
        set => SetValue(TransactionCategoryIDProperty, value);
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => (IReadOnlyDictionary<long, TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public Brush CircleColor
    {
        get => (Brush)GetValue(CircleColorProperty);
        set => SetValue(CircleColorProperty, value);
    }

    public int Amount
    {
        get => (int)GetValue(AmountProperty);
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

    private static void OnPlannedTransactionSettingInfoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateFromPlannedTransactionSettingInfo();
        }
    }

    private static void OnTransactionCategoryIDOrCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateCircleColor();
        }
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
        UpdateCircleColor();
    }

    private void UpdateFromPlannedTransactionSettingInfo()
    {
        var info = PlannedTransactionSettingInfo;
        if (info is null)
            return;

        TitleText = info.Name;
        Amount = info.Amount;
        TransactionCategoryID = info.TransactionCategoryID;
        IsIconVisible = info.IsAutomatic;
        DescriptionText = PeriodDescriptionFormatter.GetDescription(info.Period);
        
        UpdateAmountProperties(info.ChangeType);
        UpdateCircleColor();
    }

    private void UpdateCircleColor()
    {
        if (!TransactionCategoryID.HasValue || Categories is null || Categories.Count == 0)
        {
            CircleColor = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);
            return;
        }

        if (Categories.TryGetValue(TransactionCategoryID.Value, out var category) && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleColor = new SolidColorBrush(color);
        else
            CircleColor = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);
    }

    private void UpdateAmountProperties(TransactionChangeType? changeType = null)
    {
        var amount = Amount;
        var formattedAmount = AmountFormatter.FormatAmount(Math.Abs(amount));

        var actualChangeType = changeType ?? (amount >= 0 ? TransactionChangeType.Income : TransactionChangeType.Withdrawal);
        var amountSign = actualChangeType == TransactionChangeType.Income ? '+' : '-';

        AmountText = $"{amountSign} {formattedAmount} ₽";
        AmountTextColor = (actualChangeType == TransactionChangeType.Income ? App.Current?.Resources.GetColor("TransactionPositive") : App.Current?.Resources.GetColor("Gray1")) ?? AmountTextColor;
    }
}
