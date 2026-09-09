using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.PlannedTransactions;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public class AccountTabViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;
    private readonly IPlannedTransactionSettingsService _plannedTransactionSettingsService;
    
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

    public AccountTabViewModel(
        IApplicationContext applicationContext,
        IPlannedTransactionSettingsService plannedTransactionSettingsService)
    {
        _applicationContext = applicationContext;
        _plannedTransactionSettingsService = plannedTransactionSettingsService;
        
        AddScheduledTransactionCommand = new Command(OnAddScheduledTransaction);
        
        _applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        
        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);
        
        _ = LoadAutoPlannedTransactions();
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = new Dictionary<long, TransactionCategoryDto>();
            return;
        }

        Categories = newValue.ToDictionary(c => c.ID);
    }

    private async Task LoadAutoPlannedTransactions()
    {
        var settings = await _plannedTransactionSettingsService.GetInfo(CancellationToken.None);
        AutoPlannedTransactions = new ObservableCollection<PlannedTransactionSettingInfo>(settings);
    }

    private void OnAddScheduledTransaction()
    {
        // TODO: Implement command logic later
    }
}
