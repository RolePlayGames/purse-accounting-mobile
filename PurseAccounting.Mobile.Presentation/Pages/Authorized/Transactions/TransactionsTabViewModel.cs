using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionsTabViewModel : ReactiveObject
{
    private const int InitialTransactionCount = 20;

    private readonly IApplicationContext _applicationContext;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private IReadOnlyCollection<TransactionCategoryDto> _categories = [];
    private IReadOnlyDictionary<long, TransactionCategoryDto> _categoriesById = new Dictionary<long, TransactionCategoryDto>();
    private IReadOnlyCollection<long> _selectedCategoryIds = [];
    private IReadOnlyCollection<TransactionGroup>? _groupedTransactions;
    private bool _isUpdating;
    private CancellationTokenSource? _cancellationTokenSource;

    public IReadOnlyCollection<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> CategoriesById
    {
        get => _categoriesById;
        set => this.RaiseAndSetIfChanged(ref _categoriesById, value, nameof(Categories));
    }

    public IReadOnlyCollection<long> SelectedCategoryIds
    {
        get => _selectedCategoryIds;
        set => this.RaiseAndSetIfChanged(ref _selectedCategoryIds, value, nameof(SelectedCategoryIds));
    }

    public IReadOnlyCollection<TransactionGroup>? GroupedTransactions
    {
        get => _groupedTransactions;
        set => this.RaiseAndSetIfChanged(ref _groupedTransactions, value, nameof(GroupedTransactions));
    }

    public ObservableCollection<TransactionGroupViewModel> DisplayedGroups { get; } = [];

    public ICommand LoadMoreCommand { get; }

    public bool HasMoreGroupsToLoad => _groupedTransactions is not null && DisplayedGroups.Count < _groupedTransactions.Count;

    public TransactionsTabViewModel(IApplicationContext applicationContext, ITransactionService transactionService, INotificationService notificationService)
    {
        _applicationContext = applicationContext;
        _transactionService = transactionService;
        _notificationService = notificationService;

        _applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        _applicationContext.AccountChanged += OnAccountChanged;

        LoadMoreCommand = ReactiveCommand.Create(LoadMoreGroups, this.WhenAnyValue(x => x.HasMoreGroupsToLoad));

        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);

        this.WhenAnyValue(x => x.SelectedCategoryIds)
            .Subscribe(_ => LoadTransactions());
    }

    public void LoadMoreGroups()
    {
        if (_isUpdating || _groupedTransactions is null)
            return;

        if (DisplayedGroups.Count >= _groupedTransactions.Count)
            return;

        _isUpdating = true;

        try
        {
            AddGroupsToDisplayed(_groupedTransactions.Skip(DisplayedGroups.Count));
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void ResetDisplayedGroups()
    {
        if (_isUpdating)
            return;

        _isUpdating = true;

        try
        {
            foreach (var group in DisplayedGroups)
            {
                group.GroupBecameEmpty -= OnGroupBecameEmpty;
                group.Dispose();
            }

            DisplayedGroups.Clear();

            if (_groupedTransactions is not null && _groupedTransactions.Count != 0)
                AddGroupsToDisplayed(_groupedTransactions);
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void AddGroupsToDisplayed(IEnumerable<TransactionGroup> groups)
    {
        var loadedTransactionCount = 0;

        foreach (var group in groups)
        {
            var viewModel = new TransactionGroupViewModel(group, _transactionService, _notificationService);
            viewModel.GroupBecameEmpty += OnGroupBecameEmpty;

            DisplayedGroups.Add(viewModel);

            loadedTransactionCount += group.Transactions.Count;

            if (loadedTransactionCount >= InitialTransactionCount || DisplayedGroups.Count >= _groupedTransactions?.Count)
                break;
        }
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        LoadTransactions();
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = [];
            CategoriesById = new Dictionary<long, TransactionCategoryDto>();
            SelectedCategoryIds = [];
            return;
        }

        Categories = newValue;
        CategoriesById = newValue.ToDictionary(c => c.ID);

        SelectedCategoryIds = CategoriesById.Keys
            .Where(id => SelectedCategoryIds.Contains(id))
            .ToHashSet();
    }

    private void LoadTransactions()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        var cancellationToken = _cancellationTokenSource.Token;
        var timeZone = _applicationContext.Account?.TimeZone ?? 0;

        Task.Run(async () =>
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByDate(SelectedCategoryIds, timeZone, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    GroupedTransactions = transactions;
                    ResetDisplayedGroups();
                }
            }
            catch (OperationCanceledException)
            {
                // Запрос был отменён, игнорируем
            }
        }, cancellationToken);
    }

    private void OnGroupBecameEmpty(TransactionGroupViewModel viewModel)
    {
        if (_isUpdating)
            return;

        _isUpdating = true;

        try
        {
            DisplayedGroups.Remove(viewModel);
            viewModel.GroupBecameEmpty -= OnGroupBecameEmpty;
            viewModel.Dispose();
        }
        finally
        {
            _isUpdating = false;
        }
    }
}
