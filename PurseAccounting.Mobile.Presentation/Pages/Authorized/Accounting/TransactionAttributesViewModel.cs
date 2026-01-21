using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public class TransactionAttributesViewModel : ReactiveObject, IDisposable
{
    private readonly INotificationService _notificationService;
    private readonly CompositeDisposable _disposables = new();

    private TransactionChangeType _transactionChangeType;
    private TransactionChangeAmountType _transactionChangeAmountType;

    private long? _selectedCategoryId = null;
    private IList<TransactionCategoryDto> _categories = [];

    private int? _transactionAmount = null;
    private bool _isMakeTransactionEnabled = false;

    private bool _isDisposed = false;

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

    public bool IsMakeTransactionEnabled
    {
        get => _isMakeTransactionEnabled;
        set => this.RaiseAndSetIfChanged(ref _isMakeTransactionEnabled, value, nameof(IsMakeTransactionEnabled));
    }

    public ReactiveCommand<Unit, Unit> OnAmountSubmit { get; }

    public TransactionAttributesViewModel(ITransactionCategoriesService transactionCategoriesService, IApplicationContext applicationContext, INotificationService notificationService)
    {
        _notificationService = notificationService;

        TransactionChangeType = TransactionChangeType.Withdrawal;
        TransactionChangeAmountType = TransactionChangeAmountType.Daily;

        applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);

        Task.Run(() => transactionCategoriesService.LoadCategories(CancellationToken.None));

        OnAmountSubmit = ReactiveCommand.CreateFromTask(SubmitTransaction);

        this.WhenAnyValue(x => x.TransactionAmount, x => x.SelectedCategoryId)
            .Subscribe(_ => UpdateMakeTransactionEnabled())
            .DisposeWith(_disposables);
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            _disposables?.Dispose();
            GC.SuppressFinalize(this);
        }
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

    private void UpdateMakeTransactionEnabled()
    {
        IsMakeTransactionEnabled = TransactionAmount.HasValue && TransactionAmount.Value > 0 && SelectedCategoryId.HasValue;
    }

    private async Task SubmitTransaction()
    {
        if (_selectedCategoryId is null || _transactionAmount is null)
            return;

        var result = MakeTransactionResult.Success;

        // var result = await _transactionService.MakeTransaction(new()
        // {
        //    Amount = _transactionAmount.Value,
        //    ChangeType = _transactionChangeType,
        //    ChangeAmountType = _transactionChangeAmountType,
        //    TransactionCategoryID = _selectedCategoryId.Value,
        //    TransactionDate = new(),
        // }, CancellationToken.None);
        if (result == MakeTransactionResult.Success)
        {
            _notificationService.ShowSuccess("Транзакция прошла успешно");
            TransactionAmount = null;
        }
        else
        {
            var message = result switch
            {
                MakeTransactionResult.NegativeRestAmount => "Доступная сумма должна быть положительной",
                MakeTransactionResult.PlannedDateHasPassed => "Количество дней до даты планирования должно быть больше одного",
                _ => "Непредвиденная ошибка",
            };

            _notificationService.ShowError(message);
        }
    }
}
