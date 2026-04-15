using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using TransactionGroupModel = PurseAccounting.Mobile.Application.Transactions.TransactionGroup;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionGroup : ContentView
{
    public static readonly BindableProperty GroupProperty =
        BindableProperty.Create(nameof(Group), typeof(TransactionGroupModel), typeof(TransactionGroup), default(TransactionGroupModel), propertyChanged: OnGroupChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyCollection<TransactionCategoryDto>), typeof(TransactionGroup), default(IReadOnlyCollection<TransactionCategoryDto>));

    public static readonly BindableProperty DateTextProperty =
        BindableProperty.Create(nameof(DateText), typeof(string), typeof(TransactionGroup), string.Empty);

    public static readonly BindableProperty TransactionsProperty =
        BindableProperty.Create(nameof(Transactions), typeof(IReadOnlyCollection<TransactionInfo>), typeof(TransactionGroup), default(IReadOnlyCollection<TransactionInfo>));

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

    private void UpdateProperties()
    {
        if (Group == null)
            return;

        DateText = GetDateText(Group.GroupDate);
        Transactions = Group.Transactions;
    }

    private static string GetDateText(DateTime groupDate)
    {
        var today = DateTime.Today;
        
        if (groupDate.Date == today)
        {
            // Сегодня: число и название месяца, после ставить запятую и писать "сегодня"
            return $"{groupDate:dd MMMM}, сегодня";
        }
        else if (groupDate.Date == today.AddDays(-1))
        {
            // Вчера: число и название месяца, после ставить запятую и писать "вчера"
            return $"{groupDate:dd MMMM}, вчера";
        }
        else if (groupDate.Year == today.Year)
        {
            // В этом году: число и название месяца, после ставить запятую и писать день недели
            return $"{groupDate:dd MMMM}, {groupDate:dddd}";
        }
        else
        {
            // В другом году: DD.MM.YYYY, после ставить запятую и писать день недели
            return $"{groupDate:dd.MM.yyyy}, {groupDate:dddd}";
        }
    }
}
