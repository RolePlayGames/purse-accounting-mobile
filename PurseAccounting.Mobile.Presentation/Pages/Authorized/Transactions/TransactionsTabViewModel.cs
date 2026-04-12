using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionsTabViewModel : ReactiveObject
{
    private const int InitialGroupsCount = 5;
    private const int LoadMoreStep = 5;

    private readonly IApplicationContext _applicationContext;
    private readonly ITransactionService _transactionService;
    private readonly ObservableCollection<IGrouping<DateTime, DateWithTimeZone>> _displayedGroups = [];
    private IReadOnlyCollection<TransactionCategoryDto> _categories = [];
    private IReadOnlyCollection<long> _selectedCategoryIds = [];
    private IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>? _groupedTransactions;
    private int _currentGroupsCount;
    private bool _isUpdating;
    private CancellationTokenSource? _cancellationTokenSource;

    public IReadOnlyCollection<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public IReadOnlyCollection<long> SelectedCategoryIds
    {
        get => _selectedCategoryIds;
        set => this.RaiseAndSetIfChanged(ref _selectedCategoryIds, value, nameof(SelectedCategoryIds));
    }

    public IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>? GroupedTransactions
    {
        get => _groupedTransactions;
        set => this.RaiseAndSetIfChanged(ref _groupedTransactions, value, nameof(GroupedTransactions));
    }

    public ObservableCollection<IGrouping<DateTime, DateWithTimeZone>> DisplayedGroups
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

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        LoadTransactions();
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = [];
            SelectedCategoryIds = [];
            return;
        }

        Categories = newValue.Where(x => x.IsActive).ToList();

        var selectedItems = Categories
            .Where(x => SelectedCategoryIds.Contains(x.ID))
            .Select(x => x.ID)
            .ToList();

        SelectedCategoryIds = selectedItems.Count > 0 ? selectedItems : [Categories.First().ID];
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
            {
                return;
            }

            var groupsToLoad = Math.Min(InitialGroupsCount, _groupedTransactions.Count);

            foreach (var group in _groupedTransactions.Take(groupsToLoad))
            {
                _displayedGroups.Add(group);
            }

            _currentGroupsCount = groupsToLoad;
        }
        finally
        {
            _isUpdating = false;
        }
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

            for (int i = 0; i < groupsToLoad; i++)
            {
                _displayedGroups.Add(_groupedTransactions.ElementAt(_currentGroupsCount + i));
            }

            _currentGroupsCount += groupsToLoad;
        }
        finally
        {
            _isUpdating = false;
        }
    }
}
