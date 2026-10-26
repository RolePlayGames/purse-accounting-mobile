using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Pages.Authorized.PlannedTransactionSettings;

public class PlannedTransactionAttributesWidgetViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;
    private readonly INavigator _navigator;

    private string _transactionName = string.Empty;
    private int? _amount = null;
    private TransactionChangeType _transactionChangeType = TransactionChangeType.Withdrawal;
    private IReadOnlyList<TransactionCategoryDto> _categories = [];
    private long? _selectedCategoryId = null;
    private bool _isSaveEnabled = false;
    private bool _isAutoConduct = false;
    private PeriodInfo _period = new OncePeriodInfo { Date = DateTime.Today };
    private DateTime? _plannedDate = DateTime.Today;

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

    /// <summary>
    /// Флаг автоматического проведения транзакции (переключатель "Автоматически проводить").
    /// </summary>
    public bool IsAutoConduct
    {
        get => _isAutoConduct;
        set => this.RaiseAndSetIfChanged(ref _isAutoConduct, value, nameof(IsAutoConduct));
    }

    /// <summary>
    /// Выбранный тип периода плановой транзакции. Значение по умолчанию — OncePeriodInfo ("Никогда").
    /// </summary>
    public PeriodInfo Period
    {
        get => _period;
        set => this.RaiseAndSetIfChanged(ref _period, value, nameof(Period));
    }

    /// <summary>
    /// Планируемая дата транзакции. Используется при периоде OncePeriodInfo ("Никогда").
    /// </summary>
    public DateTime? PlannedDate
    {
        get => _plannedDate;
        set => this.RaiseAndSetIfChanged(ref _plannedDate, value, nameof(PlannedDate));
    }

    public ICommand SaveCommand { get; }

    public ICommand CancelCommand { get; }

    public PlannedTransactionAttributesWidgetViewModel(
        IApplicationContext applicationContext,
        INavigator navigator)
    {
        _applicationContext = applicationContext;
        _navigator = navigator;

        SaveCommand = new Command(OnSave, () => IsSaveEnabled);
        CancelCommand = new Command(OnCancel);

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

    private void OnCancel()
    {
        _navigator.ChangeTabTo<AccountTab>(CancellationToken.None).ConfigureAwait(false);
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
