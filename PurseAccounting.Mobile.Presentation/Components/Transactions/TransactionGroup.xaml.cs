using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionGroup : ContentView
{
    public static readonly BindableProperty ViewModelProperty =
        BindableProperty.Create(nameof(ViewModel), typeof(TransactionGroupViewModel), typeof(TransactionGroup), default(TransactionGroupViewModel), propertyChanged: OnViewModelChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(TransactionGroup), default(IReadOnlyDictionary<long, TransactionCategoryDto>));

    public static readonly BindableProperty DateTextProperty =
        BindableProperty.Create(nameof(DateText), typeof(string), typeof(TransactionGroup), string.Empty);

    public static readonly BindableProperty TransactionsProperty =
        BindableProperty.Create(nameof(Transactions), typeof(ObservableCollection<TransactionInfo>), typeof(TransactionGroup), default(ObservableCollection<TransactionInfo>));

    public TransactionGroupViewModel? ViewModel
    {
        get => (TransactionGroupViewModel?)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
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
    }

    private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionGroup group)
        {
            group.UpdateProperties();
        }
    }

    private void OnTransactionSwipeCompleted(object? sender, TransactionSwipedEventArgs e)
    {
        if (ViewModel?.CancelTransactionCommand is ICommand command && command.CanExecute(e.Transaction))
        {
            command.Execute(e.Transaction);
        }
    }

    private void UpdateProperties()
    {
        if (ViewModel is null)
            return;

        DateText = ViewModel.DateText;
        Transactions = ViewModel.Transactions;
    }
}
