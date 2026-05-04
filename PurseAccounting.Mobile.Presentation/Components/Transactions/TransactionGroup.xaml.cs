using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using System.Globalization;
using TransactionGroupModel = PurseAccounting.Mobile.Application.Transactions.TransactionGroup;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionGroup : ContentView
{
    public static readonly BindableProperty GroupProperty =
        BindableProperty.Create(nameof(Group), typeof(TransactionGroupModel), typeof(TransactionGroup), default(TransactionGroupModel), propertyChanged: OnGroupChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(TransactionGroup), default(IReadOnlyDictionary<long, TransactionCategoryDto>));

    public static readonly BindableProperty DateTextProperty =
        BindableProperty.Create(nameof(DateText), typeof(string), typeof(TransactionGroup), string.Empty);

    public static readonly BindableProperty TransactionsProperty =
        BindableProperty.Create(nameof(Transactions), typeof(ObservableCollection<TransactionInfo>), typeof(TransactionGroup), default(ObservableCollection<TransactionInfo>));

    public event EventHandler<TransactionSwipedEventArgs>? TransactionSwiped;

    private readonly ITransactionService? _transactionService;
    private readonly IApplicationContext? _applicationContext;

    public TransactionGroupModel Group
    {
        get => (TransactionGroupModel)GetValue(GroupProperty);
        set => SetValue(GroupProperty, value);
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => (IReadOnlyDictionary<long, TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public string DateText
    {
        get => (string)GetValue(DateTextProperty);
        set => SetValue(DateTextProperty, value);
    }

    public ObservableCollection<TransactionInfo> Transactions
    {
        get => (ObservableCollection<TransactionInfo>)GetValue(TransactionsProperty);
        set => SetValue(TransactionsProperty, value);
    }

    public TransactionGroup()
    {
        InitializeComponent();
        _transactionService = App.Current?.Handler?.MauiContext?.Services.GetService<ITransactionService>();
        _applicationContext = App.Current?.Handler?.MauiContext?.Services.GetService<IApplicationContext>();
    }

    private void OnTransactionSwipeCompleted(object? sender, TransactionSwipedEventArgs e)
    {
        HandleTransactionSwipe(e.Transaction);
    }

    private async void HandleTransactionSwipe(TransactionInfo transaction)
    {
        if (_transactionService is null || _applicationContext is null)
            return;

        var changeAmountType = transaction.ChangeAmountType == "Daily" 
            ? TransactionChangeAmountType.Daily 
            : TransactionChangeAmountType.Total;

        var result = await _transactionService.CancelTransaction(transaction.ID, changeAmountType, CancellationToken.None);

        if (result && _applicationContext.Account is not null)
        {
            // Удаляем транзакцию из ObservableCollection, что автоматически обновит UI
            var transactionToRemove = Transactions.FirstOrDefault(t => t.ID == transaction.ID);
            if (transactionToRemove is not null)
            {
                Transactions.Remove(transactionToRemove);
            }
        }
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
        if (Group is null)
            return;

        DateText = GetDateText(Group.GroupDate);
        Transactions = new ObservableCollection<TransactionInfo>(Group.Transactions);
    }
}
