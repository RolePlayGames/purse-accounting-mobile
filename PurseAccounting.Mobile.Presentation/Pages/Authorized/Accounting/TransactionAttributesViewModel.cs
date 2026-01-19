using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Reactive;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public class TransactionAttributesViewModel : ReactiveObject
{
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;

    private TransactionChangeType _transactionChangeType;
    private TransactionChangeAmountType _transactionChangeAmountType;

    private long? _selectedCategoryId = null;
    private IList<TransactionCategoryDto> _categories = [];

    private int? _transactionAmount = null;

    public TransactionChangeType TransactionChangeType
    {
        get => _transactionChangeType;
        set => this.RaiseAndSetIfChanged(ref _transactionChangeType, value, nameof(TransactionChangeType));
    }

    public TransactionChangeAmountType TransactionChangeAmountType
    {
        get => _transactionChangeAmountType;
        set => this.RaiseAndSetIfChanged(ref _transactionChangeAmountType, value, nameof(TransactionChangeAmountType));
    }

    public IList<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public long? SelectedCategoryId
    {
        get => _selectedCategoryId;
        set => this.RaiseAndSetIfChanged(ref _selectedCategoryId, value, nameof(SelectedCategoryId));
    }

    public int? TransactionAmount
    {
        get => _transactionAmount;
        set => this.RaiseAndSetIfChanged(ref _transactionAmount, value, nameof(TransactionAmount));
    }

    public ReactiveCommand<Unit, Unit> OnAmountSubmit { get; }

    public TransactionAttributesViewModel(ITransactionCategoriesService transactionCategoriesService, IApplicationContext applicationContext, ITransactionService transactionService, INotificationService notificationService)
    {
        _transactionService = transactionService;
        _notificationService = notificationService;

        TransactionChangeType = TransactionChangeType.Withdrawal;
        TransactionChangeAmountType = TransactionChangeAmountType.Daily;

        applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);

        Task.Run(() => transactionCategoriesService.LoadCategories(CancellationToken.None));

        OnAmountSubmit = ReactiveCommand.CreateFromTask(SubmitTransaction);
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = [];
            _selectedCategoryId = null;
            return;
        }

        Categories = newValue.Where(x => x.IsActive).ToList();

        var selectedItem = Categories.FirstOrDefault(x => x.ID == SelectedCategoryId);

        if (selectedItem is null)
            SelectedCategoryId = (Categories.FirstOrDefault(x => x.IsDefault) ?? Categories.First()).ID;
    }

    private async Task SubmitTransaction()
    {
        if (_selectedCategoryId is null || _transactionAmount is null)
            return;

        var isSuccseed = await _transactionService.MakeTransaction(new()
        {
            Amount = _transactionAmount.Value,
            ChangeType = _transactionChangeType,
            ChangeAmountType = _transactionChangeAmountType,
            TransactionCategoryID = _selectedCategoryId.Value,
            TransactionDate = new(),
        }, CancellationToken.None);

        if (isSuccseed)
        {
            _notificationService.ShowSuccess("Транзакция прошла успешно");
            TransactionAmount = null;
        }
        else
        {
            _notificationService.ShowError("Непредвиденная ошибка");
        }
    }
}
