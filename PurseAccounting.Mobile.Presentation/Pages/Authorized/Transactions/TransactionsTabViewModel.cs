using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionsTabViewModel : ReactiveObject
{
    private const int InitialTransactionCount = 15;
    private const int LoadMoreTransactionCount = 15;

    private readonly IApplicationContext _applicationContext;
    private readonly ITransactionService _transactionService;
    private readonly ObservableCollection<TransactionGroupViewModel> _displayedGroups = [];

    private IReadOnlyCollection<TransactionCategoryDto> _categories = [];
    private IReadOnlyDictionary<long, TransactionCategoryDto> _categoriesById = new Dictionary<long, TransactionCategoryDto>();
    private IReadOnlyCollection<long> _selectedCategoryIds = [];
    private IReadOnlyCollection<TransactionGroup>? _groupedTransactions;
    private int _currentGroupsCount;
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

    public ObservableCollection<TransactionGroupViewModel> DisplayedGroups
    {
        get => _displayedGroups;
    }

    public bool HasMoreGroupsToLoad => _groupedTransactions != null && _currentGroupsCount < _groupedTransactions.Count;

    public TransactionsTabViewModel(
        IApplicationContext applicationContext,
        ITransactionService transactionService)
    {
        _applicationContext = applicationContext;
        _transactionService = transactionService;
        _applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        _applicationContext.AccountChanged += OnAccountChanged;

        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);

        this.WhenAnyValue(x => x.SelectedCategoryIds)
            .Subscribe(_ => LoadTransactions());
    }

    public void LoadMoreGroups()
    {
        if (_isUpdating || _groupedTransactions == null)
            return;

        if (_currentGroupsCount >= _groupedTransactions.Count)
            return;

        _isUpdating = true;

        try
        {
            var loadedTransactionCount = 0;

            foreach (var group in _groupedTransactions.Skip(_currentGroupsCount))
            {
                var viewModel = new TransactionGroupViewModel(group, _transactionService);
                viewModel.GroupBecameEmpty += OnGroupBecameEmpty;
                _displayedGroups.Add(viewModel);
                loadedTransactionCount += group.Transactions.Count;
                _currentGroupsCount++;

                if (loadedTransactionCount >= LoadMoreTransactionCount || 
                    _currentGroupsCount >= _groupedTransactions.Count)
                    break;
            }
        }
        finally
        {
            _isUpdating = false;
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
                var transactions = await _transactionService.GetTransactionsByDate(
                    SelectedCategoryIds,
                    timeZone,
                    cancellationToken);

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

    private void ResetDisplayedGroups()
    {
        if (_isUpdating)
            return;

        _isUpdating = true;

        try
        {
            _displayedGroups.Clear();
            _currentGroupsCount = 0;

            if (_groupedTransactions == null || _groupedTransactions.Count == 0)
                return;

            var loadedTransactionCount = 0;

            foreach (var group in _groupedTransactions)
            {
                var viewModel = new TransactionGroupViewModel(group, _transactionService);
                viewModel.GroupBecameEmpty += OnGroupBecameEmpty;
                _displayedGroups.Add(viewModel);
                loadedTransactionCount += group.Transactions.Count;
                _currentGroupsCount++;

                if (loadedTransactionCount >= InitialTransactionCount || 
                    _currentGroupsCount >= _groupedTransactions.Count)
                    break;
            }
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private void OnGroupBecameEmpty(TransactionGroupViewModel viewModel)
    {
        if (_isUpdating)
            return;

        _isUpdating = true;

        try
        {
            _displayedGroups.Remove(viewModel);
            viewModel.GroupBecameEmpty -= OnGroupBecameEmpty;
        }
        finally
        {
            _isUpdating = false;
        }
    }
}
