using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Pages.Authorized.PlannedTransactionSettings;

public class PlannedTransactionAttributesWidgetViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;

    private string _transactionName = string.Empty;
    private int? _amount = null;
    private TransactionChangeType _transactionChangeType = TransactionChangeType.Withdrawal;
    private IReadOnlyList<TransactionCategoryDto> _categories = [];
    private long? _selectedCategoryId = null;
    private bool _isSaveEnabled = false;

    public string TransactionName
    {
        get => _transactionName;
        set => this.RaiseAndSetIfChanged(ref _transactionName, value, nameof(TransactionName));
    }

    public int? Amount
    {
        get => _amount;
        set => this.RaiseAndSetIfChanged(ref _amount, value, nameof(Amount));
    }

    public TransactionChangeType TransactionChangeType
    {
        get => _transactionChangeType;
        set => this.RaiseAndSetIfChanged(ref _transactionChangeType, value, nameof(TransactionChangeType));
    }

    public IReadOnlyList<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public long? SelectedCategoryId
    {
        get => _selectedCategoryId;
        set => this.RaiseAndSetIfChanged(ref _selectedCategoryId, value, nameof(SelectedCategoryId));
    }

    public bool IsSaveEnabled
    {
        get => _isSaveEnabled;
        set => this.RaiseAndSetIfChanged(ref _isSaveEnabled, value, nameof(IsSaveEnabled));
    }

    public ICommand SaveCommand { get; }

    public PlannedTransactionAttributesWidgetViewModel(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;

        SaveCommand = new Command(OnSave, () => IsSaveEnabled);

        this.WhenAnyValue(x => x.TransactionName, x => x.Amount, x => x.SelectedCategoryId)
            .Subscribe(_ => ValidateForm());

        applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);
    }

    private void ValidateForm()
    {
        var isNameValid = !string.IsNullOrWhiteSpace(TransactionName);
        var isAmountValid = Amount > 0;
        var isCategoryValid = SelectedCategoryId.HasValue;

        IsSaveEnabled = isNameValid && isAmountValid && isCategoryValid;
    }

    private void OnSave()
    {
        // TODO: сохранение запланированной транзакции через IPlannedTransactionSettingsService
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = [];
            SelectedCategoryId = null;
            return;
        }

        Categories = newValue.Where(x => x.IsActive).ToList();

        var selectedItem = Categories.FirstOrDefault(x => x.ID == SelectedCategoryId);

        if (selectedItem is null)
            SelectedCategoryId = (Categories.FirstOrDefault(x => x.IsDefault) ?? Categories.First()).ID;
    }
}
