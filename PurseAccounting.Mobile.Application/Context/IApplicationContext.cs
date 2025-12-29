using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;

namespace PurseAccounting.Mobile.Application.Context;

public delegate void ValueChangedEventHandler<T>(T? oldValue, T? newValue) where T : class;

public interface IApplicationContext
{
    Account? Account { get; set; }

    IReadOnlyCollection<TransactionCategoryDto> TransactionCategories { get; set; }

    event ValueChangedEventHandler<Account> AccountChanged;

    event ValueChangedEventHandler<IReadOnlyCollection<TransactionCategoryDto>> TransactionCategoriesChanged;
}
