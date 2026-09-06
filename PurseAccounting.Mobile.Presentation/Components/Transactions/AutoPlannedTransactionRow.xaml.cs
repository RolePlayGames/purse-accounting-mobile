using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
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

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(AutoPlannedTransactionRow), default(IReadOnlyDictionary<long, TransactionCategoryDto>), propertyChanged: OnCategoriesChanged);

    public PlannedTransactionSettingInfo? PlannedTransactionSettingInfo
    {
        get => (PlannedTransactionSettingInfo?)GetValue(PlannedTransactionSettingInfoProperty);
        set => SetValue(PlannedTransactionSettingInfoProperty, value);
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => (IReadOnlyDictionary<long, TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    private static void OnPlannedTransactionSettingInfoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateFromPlannedTransactionSettingInfo();
        }
    }

    private static void OnCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateCircleColor();
        }
    }

    public AutoPlannedTransactionRow()
    {
        InitializeComponent();
        UpdateState();
    }

    private void UpdateFromPlannedTransactionSettingInfo()
    {
        var info = PlannedTransactionSettingInfo;
        if (info is null)
            return;

        TitleLabel.Text = info.Name;
        IconContainer.IsVisible = info.IsAutomatic;
        DescriptionLabel.Text = PeriodDescriptionFormatter.GetDescription(info.Period);
        
        UpdateAmountProperties(info.ChangeType);
        UpdateCircleColor();
    }

    private void UpdateCircleColor()
    {
        var info = PlannedTransactionSettingInfo;
        if (info is null || Categories is null || Categories.Count == 0)
        {
            CircleElement.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);
            return;
        }

        if (Categories.TryGetValue(info.TransactionCategoryID, out var category) && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleElement.Fill = new SolidColorBrush(color);
        else
            CircleElement.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);
    }

    private void UpdateAmountProperties(TransactionChangeType? changeType = null)
    {
        var info = PlannedTransactionSettingInfo;
        if (info is null)
        {
            AmountLabel.Text = string.Empty;
            AmountLabel.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            return;
        }

        var amount = info.Amount;
        var formattedAmount = AmountFormatter.FormatAmount(Math.Abs(amount));

        var actualChangeType = changeType ?? (amount >= 0 ? TransactionChangeType.Income : TransactionChangeType.Withdrawal);
        var amountSign = actualChangeType == TransactionChangeType.Income ? '+' : '-';

        AmountLabel.Text = $"{amountSign} {formattedAmount} ₽";
        AmountLabel.TextColor = (actualChangeType == TransactionChangeType.Income ? App.Current?.Resources.GetColor("TransactionPositive") : App.Current?.Resources.GetColor("Gray1")) ?? Microsoft.Maui.Graphics.Colors.Black;
    }

    private void UpdateState()
    {
        BindingContext = this;
    }
}
