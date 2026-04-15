using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionRow : ContentView
{
    public static readonly BindableProperty TransactionProperty =
        BindableProperty.Create(nameof(Transaction), typeof(TransactionInfo), typeof(TransactionRow), default(TransactionInfo), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyCollection<TransactionCategoryDto>), typeof(TransactionRow), default(IReadOnlyCollection<TransactionCategoryDto>), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(TransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public static readonly BindableProperty AmountTextProperty =
        BindableProperty.Create(nameof(AmountText), typeof(string), typeof(TransactionRow), string.Empty);

    public static readonly BindableProperty AmountTextColorProperty =
        BindableProperty.Create(nameof(AmountTextColor), typeof(Color), typeof(TransactionRow), Microsoft.Maui.Graphics.Colors.Black);

    public static readonly BindableProperty TransactionTypeTextProperty =
        BindableProperty.Create(nameof(TransactionTypeText), typeof(string), typeof(TransactionRow), string.Empty);

    public TransactionInfo Transaction
    {
        get => (TransactionInfo)GetValue(TransactionProperty);
        set => SetValue(TransactionProperty, value);
    }

    public IReadOnlyCollection<TransactionCategoryDto> Categories
    {
        get => (IReadOnlyCollection<TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public Brush CircleColor
    {
        get => (Brush)GetValue(CircleColorProperty);
        set => SetValue(CircleColorProperty, value);
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

    public string TransactionTypeText
    {
        get => (string)GetValue(TransactionTypeTextProperty);
        set => SetValue(TransactionTypeTextProperty, value);
    }

    public TransactionRow()
    {
        InitializeComponent();
    }

    private static void OnTransactionOrCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionRow row)
        {
            row.UpdateProperties();
        }
    }

    private static string FormatAmount(double amount)
    {
        var rubles = (long)amount;
        var kopecks = (int)(((amount - rubles) * 100) + 0.5);

        if (kopecks == 0)
            return $"{rubles:N0} ₽";

        return $"{rubles:N0},{kopecks:D2} ₽";
    }

    private void UpdateProperties()
    {
        if (Transaction is null || Categories is null || Categories.Count == 0)
            return;

        var category = Categories.FirstOrDefault(c => c.ID == Transaction.TransactionCategoryID);

        if (category is not null && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleColor = new SolidColorBrush(color);
        else
            CircleColor = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);

        var amount = Transaction.Amount;
        var amountInRubles = amount / 100.0;
        var amountAbs = Math.Abs(amountInRubles);

        if (amount >= 0)
            AmountText = FormatAmount(amountAbs);
        else
            AmountText = $"+ {FormatAmount(amountAbs)}";

        TransactionTypeText = Transaction.ChangeAmountType == "Daily" ? "Ежедневная" : "Общая";
    }
}
