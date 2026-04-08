using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;
using System.Reactive.Linq;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public class TransactionsTabViewModel : ReactiveObject
{
    private IList<TransactionCategoryDto> _categories = [];
    private IList<long> _selectedCategoryIds = [];
    private IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>? _groupedTransactions;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly IApplicationContext _applicationContext;
    private readonly ITransactionService _transactionService;

    public IList<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public IList<long> SelectedCategoryIds
    {
        get => _selectedCategoryIds;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _selectedCategoryIds, value, nameof(SelectedCategoryIds)))
            {
                LoadTransactions();
            }
        }
    }

    public IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>? GroupedTransactions
    {
        get => _groupedTransactions;
        set => this.RaiseAndSetIfChanged(ref _groupedTransactions, value, nameof(GroupedTransactions));
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
    }

    private void OnAccountChanged(Account? oldValue, Account? newValue)
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
                }
            }
            catch (OperationCanceledException)
            {
                // Запрос был отменён, игнорируем
            }
        }, cancellationToken);
    }
}
