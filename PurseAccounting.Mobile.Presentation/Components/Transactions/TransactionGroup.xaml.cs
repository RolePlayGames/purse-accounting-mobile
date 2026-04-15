using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using System.Globalization;
using TransactionGroupModel = PurseAccounting.Mobile.Application.Transactions.TransactionGroup;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionGroup : ContentView
{
    public static readonly BindableProperty GroupProperty =
        BindableProperty.Create(nameof(Group), typeof(TransactionGroupModel), typeof(TransactionGroup), default(TransactionGroupModel), 
            propertyChanged: OnGroupChanged,
            defaultBindingMode: BindingMode.OneTime);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyCollection<TransactionCategoryDto>), typeof(TransactionGroup), default(IReadOnlyCollection<TransactionCategoryDto>),
            defaultBindingMode: BindingMode.OneTime);

    public static readonly BindableProperty DateTextProperty =
        BindableProperty.Create(nameof(DateText), typeof(string), typeof(TransactionGroup), string.Empty,
            defaultBindingMode: BindingMode.OneWay);

    public static readonly BindableProperty TransactionsProperty =
        BindableProperty.Create(nameof(Transactions), typeof(IReadOnlyCollection<TransactionInfo>), typeof(TransactionGroup), default(IReadOnlyCollection<TransactionInfo>),
            defaultBindingMode: BindingMode.OneTime);

    public TransactionGroupModel Group
    {
        get => (TransactionGroupModel)GetValue(GroupProperty);
        set => SetValue(GroupProperty, value);
    }

    public IReadOnlyCollection<TransactionCategoryDto> Categories
    {
        get => (IReadOnlyCollection<TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public string DateText
    {
        get => (string)GetValue(DateTextProperty);
        set => SetValue(DateTextProperty, value);
    }

    public IReadOnlyCollection<TransactionInfo> Transactions
    {
        get => (IReadOnlyCollection<TransactionInfo>)GetValue(TransactionsProperty);
        set => SetValue(TransactionsProperty, value);
    }

    public TransactionGroup()
    {
        InitializeComponent();
    }

    private static void OnGroupChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionGroup group)
        {
            group.UpdateProperties();
        }
    }

    private static string GetDateText(DateTime groupDate)
    {
        var today = DateTime.Today;
        var culture = CultureInfo.GetCultureInfo("ru-RU");

        if (groupDate.Date == today)
        {
            return $"{groupDate.ToString("d MMMM", culture)}, сегодня";
        }
        else if (groupDate.Date == today.AddDays(-1))
        {
            return $"{groupDate.ToString("d MMMM", culture)}, вчера";
        }
        else if (groupDate.Year == today.Year)
        {
            return $"{groupDate.ToString("d MMMM", culture)}, {groupDate.ToString("dddd", culture)}";
        }
        else
        {
            return $"{groupDate.ToString("dd.MM.yyyy", culture)}, {groupDate.ToString("dddd", culture)}";
        }
    }

    private void UpdateProperties()
    {
        if (Group == null)
            return;

        DateText = GetDateText(Group.GroupDate);
        Transactions = Group.Transactions;
    }
}
