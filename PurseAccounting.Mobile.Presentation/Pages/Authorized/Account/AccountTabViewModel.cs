using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public class AccountTabViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;
    
    private ObservableCollection<PlannedTransactionSettingInfo> _autoPlannedTransactions = [];

    public ObservableCollection<PlannedTransactionSettingInfo> AutoPlannedTransactions
    {
        get => _autoPlannedTransactions;
        set => this.RaiseAndSetIfChanged(ref _autoPlannedTransactions, value, nameof(AutoPlannedTransactions));
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
