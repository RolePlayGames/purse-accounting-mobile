using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionsTabViewModel : ReactiveObject
{
    private const int InitialGroupsCount = 2;
    private const int LoadMoreStep = 2;

    private readonly IApplicationContext _applicationContext;
    private readonly ITransactionService _transactionService;
    private readonly ObservableCollection<TransactionGroup> _displayedGroups = [];

    private IReadOnlyDictionary<long, TransactionCategoryDto> _categories = new Dictionary<long, TransactionCategoryDto>();
    private IReadOnlyCollection<long> _selectedCategoryIds = [];
    private IReadOnlyCollection<TransactionGroup>? _groupedTransactions;
    private int _currentGroupsCount;
    private bool _isUpdating;
    private CancellationTokenSource? _cancellationTokenSource;

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
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

    public ObservableCollection<TransactionGroup> DisplayedGroups
    {
        get => _displayedGroups;
    }

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
            var groupsToLoad = Math.Min(LoadMoreStep, _groupedTransactions.Count - _currentGroupsCount);

            foreach (var group in _groupedTransactions.Skip(_currentGroupsCount).Take(groupsToLoad))
                _displayedGroups.Add(group);

            _currentGroupsCount += groupsToLoad;
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
            Categories = new Dictionary<long, TransactionCategoryDto>();
            SelectedCategoryIds = [];
            return;
        }

        Categories = newValue.ToDictionary(c => c.ID);

        SelectedCategoryIds = Categories.Keys
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

            var groupsToLoad = Math.Min(InitialGroupsCount, _groupedTransactions.Count);

            foreach (var group in _groupedTransactions.Take(groupsToLoad))
                _displayedGroups.Add(group);

            _currentGroupsCount = groupsToLoad;
        }
        finally
        {
            _isUpdating = false;
        }
    }
}
