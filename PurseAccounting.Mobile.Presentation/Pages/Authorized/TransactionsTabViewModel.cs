using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public class TransactionsTabViewModel : ReactiveObject
{
    private IList<TransactionCategoryDto> _categories = [];
    private IList<long> _selectedCategoryIds = [];

    public IList<TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public IList<long> SelectedCategoryIds
    {
        get => _selectedCategoryIds;
        set => this.RaiseAndSetIfChanged(ref _selectedCategoryIds, value, nameof(SelectedCategoryIds));
    }

    public TransactionsTabViewModel(
        IApplicationContext applicationContext)
    {
        applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;
        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);
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
}
