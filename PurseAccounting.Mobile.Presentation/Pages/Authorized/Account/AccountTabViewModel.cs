using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public class AccountTabViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;
    
    private ObservableCollection<PlannedTransactionSettingInfo> _autoPlannedTransactions = [];
    private IReadOnlyDictionary<long, TransactionCategoryDto> _categories = new Dictionary<long, TransactionCategoryDto>();

    public ObservableCollection<PlannedTransactionSettingInfo> AutoPlannedTransactions
    {
        get => _autoPlannedTransactions;
        set => this.RaiseAndSetIfChanged(ref _autoPlannedTransactions, value, nameof(AutoPlannedTransactions));
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public ICommand AddScheduledTransactionCommand { get; }

    public AccountTabViewModel(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        
        AddScheduledTransactionCommand = new Command(OnAddScheduledTransaction);
    }

    private void OnAddScheduledTransaction()
    {
        // TODO: Implement command logic later
    }
}
