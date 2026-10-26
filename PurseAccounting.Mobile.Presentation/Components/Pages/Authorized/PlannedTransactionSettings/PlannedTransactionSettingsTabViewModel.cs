using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using ReactiveUI;

namespace PurseAccountinng.Mobile.Presentation.Components.Pages.Authorized.PlannedTransactionSettings;

public class PlannedTransactionSettingsTabViewModel : ReactiveObject
{
    private readonly IApplicationContext _applicationContext;

    private IReadOnlyDictionary<long, TransactionCategoryDto> _categories = new Dictionary<long, TransactionCategoryDto>();

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value, nameof(Categories));
    }

    public PlannedTransactionSettingsTabViewModel(IApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;

        _applicationContext.TransactionCategoriesChanged += OnTransactionCategoriesChanged;

        OnTransactionCategoriesChanged(null, applicationContext.TransactionCategories);
    }

    private void OnTransactionCategoriesChanged(IReadOnlyCollection<TransactionCategoryDto>? oldValue, IReadOnlyCollection<TransactionCategoryDto>? newValue)
    {
        if (newValue is null || newValue.Count == 0)
        {
            Categories = new Dictionary<long, TransactionCategoryDto>();
            return;
        }

        Categories = newValue.ToDictionary(c => c.ID);
    }
}
